using System.Collections;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMaGD.Models;
using Line = GMaGD.Models.Line;

namespace GMaGD;

public partial class MainWindow : Window
{
    private static double _pxPerCm = 2;
    private static double _centerLineLength = 135;
    private const double LineThickness = 1;
    private const double CoordLineThickness = 0.25;
    private double AxisThickness = 3;
    private double rotationAngle;
    private double rotationX = 50;
    private double rotationY = 50;
    private double locationX = 50;
    private double locationY = 50;
    
    private double xX = 1;
    private double yX = 0;
    private double xY = 0;
    private double yY = 1;
    private double x0 = 0;
    private double y0 = 0;
    private double xXw = 150;
    private double xYw = 0;
    private double wX = 1;
    private double yXw = 0;
    private double yYw = 150;
    private double wY = 1;
    private double x0w = 0;
    private double y0w = 0;
    private double w0 = 350;
    
    private double _largeArcRadius = 35;
    private double _smallArcRadius = 16;

    private double Ab = 15;
    private double Bc = 15;
    private double Cd = 10;
    private double De = 6;
    private double Ef = 15;
    private double Gh = 15;
    private double Hi = 6;
    private double Ij = 10;
    private double Jk = 15;
    private double Kl = 15;
    
    private double Sx = 1;
    private double Sy = 1;
    private double RefX = 15;
    private double RefY = 10;

    public MainWindow()
    {
        InitializeComponent();
        
        DrawCoordinateSystem();
        DrawShape();
    }
    
    private void DrawShape()
    {
        double[][] scaleMatrix =
        [
            [Sx,  0, 0],
            [ 0, Sy, 0],
            [ 0,  0, 1]
        ];
     
        double[][] reflectMatrix =
        [
            [-1, 0, 0],
            [0, -1, 0],
            [2*RefX, 2*RefY, 1]
        ];
        
        double[][] matrixAffine =
        [
            [xX, yX, 0],
            [xY, yY, 0],
            [x0, y0, 1]
        ];
        double[][] matrixProjective =
        [
            [xXw * wX, yXw * wX, wX],
            [xYw * wY, yYw * wY, wY],
            [x0w * w0, y0w * w0, w0]
        ];

        Dot center = new Dot(locationX, locationY);
        
        Dot rotateCenter = new Dot(rotationX, rotationY);
        
        Arc smallArc = new Arc(90, 270, center, _smallArcRadius);
        Arc largeArc = new Arc(134.5, 224.5, center, _largeArcRadius);
        
        DrawDot(Px(rotateCenter.X), Px(rotateCenter.Y), _pxPerCm/3, Brushes.Blue, LineThickness);
        
        
        var fgDots = GetArcEndPoints(smallArc.Center, smallArc.Radius, smallArc.StartAngle, smallArc.EndAngle);
        var laDots = GetArcEndPoints(largeArc.Center, largeArc.Radius, largeArc.StartAngle, largeArc.EndAngle + 1);
        
        DrawDot(center.X, center.Y, 1/_pxPerCm, Brushes.Black, LineThickness);

        Dot A = new Dot(Cm(laDots.startPoint.X), Cm(laDots.startPoint.Y));
        Dot L = new Dot(Cm(laDots.endPoint.X), Cm(laDots.endPoint.Y));
        Dot G = new Dot(Cm(fgDots.endPoint.X), Cm(fgDots.endPoint.Y));
        Dot F = new Dot(Cm(fgDots.startPoint.X), Cm(fgDots.startPoint.Y));
        Dot E = new Dot(F.X + (Ef), F.Y);
        Dot H = new Dot(G.X + (Gh), G.Y);
        Dot D = new Dot(E.X, E.Y - (De));
        Dot I = new Dot(H.X, H.Y + (Hi));
        Dot C = new Dot(D.X + (Cd), D.Y);
        Dot J = new Dot(I.X + (Ij), I.Y);
        Dot B = new Dot(C.X  + Ab - 15, C.Y + (Bc));
        Dot K = new Dot(J.X + Kl - 15, J.Y - (Jk));
        
        A = pointEuclidRotation(A, rotateCenter, rotationAngle);
        L = pointEuclidRotation(L, rotateCenter, rotationAngle);
        G = pointEuclidRotation(G, rotateCenter, rotationAngle);
        F = pointEuclidRotation(F, rotateCenter, rotationAngle);
        E = pointEuclidRotation(E, rotateCenter, rotationAngle);
        H = pointEuclidRotation(H, rotateCenter, rotationAngle);
        D = pointEuclidRotation(D, rotateCenter, rotationAngle);
        I = pointEuclidRotation(I, rotateCenter, rotationAngle);
        C = pointEuclidRotation(C, rotateCenter, rotationAngle);
        J = pointEuclidRotation(J, rotateCenter, rotationAngle);
        B = pointEuclidRotation(B, rotateCenter, rotationAngle);
        K = pointEuclidRotation(K, rotateCenter, rotationAngle);
        
        center = pointEuclidRotation(center, rotateCenter, rotationAngle);

        smallArc.Center = center;
        largeArc.Center = center;
        
        DrawDot(Px(center.X), Px(center.Y), _pxPerCm/3, Brushes.Black, LineThickness);
       
        
        
        
        
        
        DrawRotatedArc(smallArc, rotationAngle, smallArc.Radius, matrixAffine, matrixProjective, scaleMatrix);
        DrawRotatedArc(largeArc, rotationAngle, largeArc.Radius, matrixAffine, matrixProjective, scaleMatrix);
       
        DrawLine(Px(C.X), Px(C.Y), Px(B.X), Px(B.Y),Brushes.Black, LineThickness);
        DrawLine(Px(J.X), Px(J.Y), Px(K.X), Px(K.Y),Brushes.Black, LineThickness);
        
        DrawLine(Px(F.X), Px(F.Y), Px(E.X), Px(E.Y), Brushes.Black, LineThickness);
        DrawLine(Px(G.X), Px(G.Y), Px(H.X), Px(H.Y), Brushes.Black, LineThickness);
        
        DrawLine(Px(D.X), Px(D.Y), Px(E.X), Px(E.Y), Brushes.Black, LineThickness);
        DrawLine(Px(I.X), Px(I.Y), Px(H.X), Px(H.Y), Brushes.Black, LineThickness);
        
        DrawLine(Px(D.X), Px(D.Y), Px(C.X), Px(C.Y), Brushes.Black, LineThickness);
        DrawLine(Px(I.X), Px(I.Y), Px(J.X), Px(J.Y), Brushes.Black, LineThickness);
       
        DrawLine(Px(K.X), Px(K.Y), Px(L.X), Px(L.Y) , Brushes.Black, LineThickness);
        DrawLine(Px(B.X), Px(B.Y), Px(A.X), Px(A.Y) , Brushes.Black, LineThickness);
        
        
        
        GetData();
    }

    private void ApplySettings_Click(object sender, RoutedEventArgs e)
    {
        double.TryParse(LargeArcRadius.Text, out _largeArcRadius);
        double.TryParse(SmallArcRadius.Text, out _smallArcRadius);
        double.TryParse(AbInput.Text, out Ab);
        double.TryParse(BcInput.Text, out Bc);
        double.TryParse(CdInput.Text, out Cd);
        double.TryParse(DeInput.Text, out De);
        double.TryParse(EfInput.Text, out Ef);
        double.TryParse(GhInput.Text, out Gh);
        
        double.TryParse(HiInput.Text, out Hi);
        double.TryParse(IjInput.Text, out Ij);
        double.TryParse(JkInput.Text, out Jk);
        double.TryParse(KlInput.Text, out Kl);
        DrawCoordinateSystem();
        DrawShape();
    }

    private void ApplyRotation_Click(object sender, RoutedEventArgs e)
    {
// Отримуємо кут обертання з поля вводу 
        double.TryParse(RotationAngleInput.Text, out rotationAngle);
        double.TryParse(RotationXInput.Text, out rotationX);
        double.TryParse(RotationYInput.Text, out rotationY);
// Оновлюємо фігури після зміни кута обертання 
        DrawCoordinateSystem();
        DrawShape();
    }

    private void ApplyLocation_Click(object sender, RoutedEventArgs e)
    {
// Отримуємо кут обертання з поля вводу 
        double.TryParse(LocationXInput.Text, out locationX);
        double.TryParse(LocationYInput.Text, out locationY);
// Оновлюємо фігури після зміни кута обертання 
        DrawCoordinateSystem();
        DrawShape();
    }

    private void ApplySettingsPxPerCm_Click(object sender, RoutedEventArgs e)
    {
// Отримуємо значення з полів вводу і перетворюємо їх у відповідні типи 
        double.TryParse(PxPerCmInput.Text, out _pxPerCm);
        DrawCoordinateSystem();
        DrawShape();
    }

    private void ApplyAffineTransformation_Click(object sender, RoutedEventArgs e)
    {
// Отримуємо значення з полів вводу і перетворюємо їх у відповідні типи 
        double.TryParse(xXInput.Text, out xX);
        double.TryParse(yXInput.Text, out yX);
        double.TryParse(xYInput.Text, out xY);
        double.TryParse(yYInput.Text, out yY);
        double.TryParse(x0Input.Text, out x0);
        double.TryParse(y0Input.Text, out y0);
        DrawCoordinateSystem();
        DrawShape();
    }

    private void ApplyProjectiveTransformation_Click(object sender, RoutedEventArgs e)
    {
// Отримуємо значення з полів вводу і перетворюємо їх у відповідні типи 
        double.TryParse(xXwInput.Text, out xXw);
        double.TryParse(yXwInput.Text, out yXw);
        double.TryParse(xYwInput.Text, out xYw);
        double.TryParse(yYwInput.Text, out yYw);
        double.TryParse(x0wInput.Text, out x0w);
        double.TryParse(y0wInput.Text, out y0w);
        double.TryParse(wYInput.Text, out wY);
        double.TryParse(wXInput.Text, out wX);
        double.TryParse(w0Input.Text, out w0);
        DrawCoordinateSystem();
        DrawShape();
    }
    
    private void ApplyReflection_Click(object sender, RoutedEventArgs e)
    {
        double.TryParse(ReflectionXInput.Text, out RefX);
        double.TryParse(ReflectionYInput.Text, out RefY);
        
        DrawCoordinateSystem();
        DrawShape();
    }
    
    private void ApplyScaling_Click(object sender, RoutedEventArgs e)
    {
// Отримуємо значення з полів вводу і перетворюємо їх у відповідні типи 
        
        double.TryParse(SxInput.Text, out Sx);
        double.TryParse(SyInput.Text, out Sy);
        
        DrawCoordinateSystem();
        DrawShape();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyCoordinateSystemTransformation();
        DrawCoordinateSystem();
        DrawShape();
    }

    private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        Point mousePosition = e.GetPosition(DrawingCanvas);
        double xCoord = mousePosition.X ;
        double yCoord = mousePosition.Y;
        double xCoordCm = xCoord / _pxPerCm;
        double yCoordCm = yCoord / _pxPerCm;
        Title = $"X/px: {xCoord:0.00}, Y/px: {yCoord:0.00} X/cm: {xCoordCm:0.00}, Y/cm: {yCoordCm:0.00} ";
    }

// Метод для малювання лінії на Canvas 
    private void DrawLine(double x1, double y1, double x2, double y2, Brush color, double thickness)
    {
// Використовуємо новий конструктор для створення лінії 
        var customLine = new Line(x1, y1, x2, y2, color);
// Створюємо WPF-лінію 
        var wpfLine = new System.Windows.Shapes.Line()
        {
            X1 = customLine.StartDot.X,
            Y1 = customLine.StartDot.Y,
            X2 = customLine.EndDot.X,
            Y2 = customLine.EndDot.Y,
            Stroke = color,
            StrokeThickness = thickness
        };
// Додаємо лінію до Canvas 
        DrawingCanvas.Children.Add(wpfLine);
    }

    private void ApplyCoordinateSystemTransformation()
    {
        TransformGroup transformGroup = new TransformGroup();
        transformGroup.Children.Add(new ScaleTransform(1, -1));
        transformGroup.Children.Add(new TranslateTransform(0, DrawingCanvas.ActualHeight));
        DrawingCanvas.RenderTransform = transformGroup;
    }

    private void DrawCoordinateSystem()
    {
        DrawingCanvas.Children.Clear();
        double[][] matrixProjective =
        [
            [xXw * wX, yXw * wX, wX],
            [xYw * wY, yYw * wY, wY],
            [x0w * w0, y0w * w0, w0]
        ];
        
        double width = DrawingCanvas.ActualWidth;
        double height = DrawingCanvas.ActualHeight;
        
        if (AffineCheckBox.IsChecked == true ||
            (AffineCheckBox.IsChecked == false && ProjectiveCheckBox.IsChecked == false))
        {
            double yCount = Px(y0);
            double yCounter = 0;
            for (double x = Px(x0); x <= width; x += _pxPerCm * xX)
            {
                DrawLine(x, yCount, x + Px(xX), yCount + Px(yX), Brushes.LimeGreen, AxisThickness);
                yCount += Px(yX);
                yCounter++;
            }

            for (double y =Px(y0); y <= height; y += _pxPerCm * yY)
            {
                DrawLine(0, y, width, y + yCounter * Px(yX), Brushes.Gray, CoordLineThickness);
            }

            double xCount = Px(x0);
            double xCounter = 0;
            for (double y =  Px(y0); y <= height; y += _pxPerCm * yY)
            {
                DrawLine(xCount, y, xCount + Px(xY), y + Px(yY), Brushes.Red, AxisThickness);
                xCount += Px(xY);
                xCounter++;
            }

            for (double x =  Px(x0); x <= width; x += _pxPerCm * xX)
            {
                DrawLine(x, 0, x + xCounter * Px(xY), height, Brushes.Gray,
                    CoordLineThickness); // Вертикальні лінії 
            }
        }
        else if (ProjectiveCheckBox.IsChecked == true)
        {
            double gridSize = _pxPerCm;
            double cellCountX = xXw;
            double cellCountY = yYw;
            for (int i = 0; i <= cellCountX; i++)
            {
                if (i == 0)
                {
                    DrawProjectiveLine(matrixProjective, 0, i * gridSize, cellCountX * gridSize, i * gridSize,
                        Brushes.LimeGreen, AxisThickness);
                    DrawProjectiveLine(matrixProjective, i * gridSize, 0, i * gridSize, cellCountY * gridSize,
                        Brushes.Red, AxisThickness);
                }
                else
                {
                    DrawProjectiveLine(matrixProjective, 0, i * gridSize, cellCountX * gridSize, i * gridSize,
                        Brushes.LightGray, LineThickness);
                    DrawProjectiveLine(matrixProjective, i * gridSize, 0, i * gridSize, cellCountY * gridSize,
                        Brushes.LightGray, LineThickness);
                }
            }
        }

        DrawDot(Px(x0), Px(y0), Px(2) / _pxPerCm, Brushes.Black, AxisThickness);
        DrawDot(Px(xX + x0), Px(yX + y0), Px(2 / _pxPerCm), Brushes.LimeGreen, AxisThickness);
        DrawDot(Px(xY + x0), Px(yY + y0), Px(2 / _pxPerCm), Brushes.Red, AxisThickness);
    }

    private void DrawProjectiveLine(double[][] matrixProjective, double x1, double y1, double x2,
        double y2, Brush color, double thickness)
    {
// Перетворюємо точки лінії за допомогою проективної матриці 
        var p1 = ProjectiveTransformation(new Dot(x1, y1), matrixProjective);
        var p2 = ProjectiveTransformation(new Dot(x2, y2), matrixProjective);
        var customLine = new Line(p1.X, p1.Y, p2.X, p2.Y, color);
// Створюємо WPF-лінію 
        var wpfLine = new System.Windows.Shapes.Line()
        {
            X1 = customLine.StartDot.X,
            Y1 = customLine.StartDot.Y,
            X2 = customLine.EndDot.X,
            Y2 = customLine.EndDot.Y,
            Stroke = color,
            StrokeThickness = thickness
        };
// Додаємо лінію до Canvas 
        DrawingCanvas.Children.Add(wpfLine);
    }

    private void DrawArc(double x, double y, double radius, double startAngle, double endAngle, Brush color,
        double thickness, double[][] matrixAffine, double[][] matrixProjective, double[][] matrixScale)
    {
        Arc arc = new Arc(startAngle, endAngle, new Dot(x, y), radius);
        for (double i = startAngle; i <= endAngle;  i += 0.5)
        {
            double rad1 = degToRad(i);
            double rad2 = degToRad(i + 1);
            Dot Pbeg = new Dot(
                arc.Center.X + arc.Radius * Math.Cos(rad1),
                arc.Center.Y - arc.Radius * Math.Sin(rad1)
            );
            Dot Pend = new Dot(
                arc.Center.X + arc.Radius * Math.Cos(rad2),
                arc.Center.Y - arc.Radius * Math.Sin(rad2)
            );

            Pbeg = ScalePoint(Pbeg, matrixScale);
            Pend = ScalePoint(Pend, matrixScale);
            
            if (AffineCheckBox.IsChecked == true)
            {
                Pbeg = AffineTransformation(Pbeg, matrixAffine);
                Pend = AffineTransformation(Pend, matrixAffine);
            }
            else if (ProjectiveCheckBox.IsChecked == true)
            {
                Pbeg = ProjectiveTransformation(Pbeg, matrixProjective);
                Pend = ProjectiveTransformation(Pend, matrixProjective);
            }

            DrawLine(Pbeg.X, Pbeg.Y, Pend.X, Pend.Y, color, thickness);
        }
    }

    private void DrawDot(double x, double y, double radius, Brush color, double thickness)
    {
        Circle circle = new Circle(new Dot(x, y), radius);
        for (int i = 0; i < 360; ++i)
        {
            double rad1 = degToRad(i);
            double rad2 = degToRad(i + 1);
            Dot Pbeg = new Dot(
                circle.Center.X + radius * Math.Cos(rad1),
                circle.Center.Y + radius * Math.Sin(rad1)
            );
            Dot Pend = new Dot(
                circle.Center.X + radius * Math.Cos(rad2),
                circle.Center.Y - radius * Math.Sin(rad2)
            );
            DrawLine(Pbeg.X, Pbeg.Y, Pend.X, Pend.Y, color, thickness);
        }
    }

    private double degToRad(double degrees)
    {
        return degrees * (Math.PI / 180);
    }

    private Dot AffineTransformation(Dot point, double[][] transformationMatrix)
    {
        double newX = transformationMatrix[0][0] * point.X + transformationMatrix[1][0] * point.Y +
                      transformationMatrix[2][0] * Px(1);
        double newY = transformationMatrix[0][1] * point.X + transformationMatrix[1][1] * point.Y +
                      transformationMatrix[2][1] * Px(1);
        return new Dot(newX, newY);
    }

    public Dot ProjectiveTransformation(Dot point, double[][] matrixProjective)
    {
// Розрахунок значень X', Y', W' 
        double XPrime = matrixProjective[0][0] * point.X + matrixProjective[1][0] * point.Y + matrixProjective[2][0];
        double YPrime = matrixProjective[0][1] * point.X + matrixProjective[1][1] * point.Y + matrixProjective[2][1];
        double WPrime = matrixProjective[0][2] * point.X + matrixProjective[1][2] * point.Y + matrixProjective[2][2];
// Отримуємо нові координати Xresult та Yresult 
        double XResult = XPrime / WPrime;
        double YResult = YPrime / WPrime;
        return new Dot(XResult, YResult);
    }

   

    private void GetData()
    {
        PxPerCmInput.Text = _pxPerCm.ToString(CultureInfo.InvariantCulture);
        SmallArcRadius.Text = _smallArcRadius.ToString(CultureInfo.InvariantCulture);
        LargeArcRadius.Text = _largeArcRadius.ToString(CultureInfo.InvariantCulture);
        
        AbInput.Text = Ab.ToString(CultureInfo.InvariantCulture);
        BcInput.Text = Bc.ToString(CultureInfo.InvariantCulture);
        CdInput.Text = Cd.ToString(CultureInfo.InvariantCulture);
        DeInput.Text = De.ToString(CultureInfo.InvariantCulture);
        EfInput.Text = Ef.ToString(CultureInfo.InvariantCulture);
        GhInput.Text = Gh.ToString(CultureInfo.InvariantCulture);
        HiInput.Text = Hi.ToString(CultureInfo.InvariantCulture);
        IjInput.Text = Ij.ToString(CultureInfo.InvariantCulture);
        JkInput.Text = Jk.ToString(CultureInfo.InvariantCulture);
        KlInput.Text = Kl.ToString(CultureInfo.InvariantCulture);
        
        
        LocationXInput.Text = locationX.ToString();
        LocationYInput.Text = locationY.ToString();
        RotationXInput.Text = rotationX.ToString();
        RotationYInput.Text = rotationY.ToString();
        xXInput.Text = xX.ToString();
        xYInput.Text = xY.ToString();
        yYInput.Text = yY.ToString();
        yXInput.Text = yX.ToString();
        x0Input.Text = x0.ToString();
        y0Input.Text = y0.ToString();
        xXwInput.Text = xXw.ToString();
        xYwInput.Text = xYw.ToString();
        wXInput.Text = wX.ToString();
        yXwInput.Text = yXw.ToString();
        yYwInput.Text = yYw.ToString();
        wYInput.Text = wY.ToString();
        x0wInput.Text = x0w.ToString();
        y0wInput.Text = y0w.ToString();
        w0Input.Text = w0.ToString();
        SxInput.Text = Sx.ToString();
        SyInput.Text = Sy.ToString();
        ReflectionXInput.Text = RefX.ToString();
        ReflectionYInput.Text = RefY.ToString();
    }

    private void DrawRotatedArc(Arc arc, double rotationAngle, double radius, double[][] matrixAffine,
        double[][] matrixProjective, double [][] scaleMatrix)
    {
// Оновлюємо кути дуги 
        double adjustedStartAngle = (arc.StartAngle + rotationAngle) % 360;
        double adjustedEndAngle = (arc.EndAngle + rotationAngle) % 360;
        if (adjustedStartAngle < 0) adjustedStartAngle += 360;
        if (adjustedEndAngle < 0) adjustedEndAngle += 360;
// Перевіряємо, чи дуга перетинає межу 360 градусів 
        if (adjustedEndAngle < adjustedStartAngle)
        {
// Малюємо дві частини дуги: від стартового кута до 360 і від 0 до кінцевого кута 
            DrawArc(Px(arc.Center.X), Px(arc.Center.Y), Px(radius), adjustedStartAngle, 360,
                Brushes.Black, LineThickness, matrixAffine, matrixProjective, scaleMatrix);
            DrawArc(Px(arc.Center.X), Px(arc.Center.Y), Px(radius), 0, adjustedEndAngle,
                Brushes.Black, LineThickness, matrixAffine, matrixProjective, scaleMatrix);
        }
        else
        {
// Малюємо дугу в межах стартового та кінцевого кута 
            DrawArc(Px(arc.Center.X), Px(arc.Center.Y), Px(radius), adjustedStartAngle,
                adjustedEndAngle, Brushes.Black, LineThickness, matrixAffine, matrixProjective, scaleMatrix);
        }
    }

    private void UpdateLineCoordinates(Dot startPoint, Dot endPoint, Brush color)
    {
        if (!startPoint.Equals(endPoint))
            DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, color, LineThickness);
    }

    private Dot pointEuclidRotation(Dot point, Dot center, double angle)
    {
        double angleInRadians = -angle * Math.PI / 180;
        double cosA = Math.Cos(angleInRadians);
        double sinA = Math.Sin(angleInRadians);
        double newX = center.X + (point.X - center.X) * cosA - (point.Y - center.Y) * sinA;
        double newY = center.Y + (point.X - center.X) * sinA + (point.Y - center.Y) * cosA;
        return new Dot(newX, newY);
    }

    private Dot ReflectPoint(Dot point, double[][] reflectMatrix)
    {
        
        double  newX = 2*RefX - point.X;
        double  newY = 2*RefY - point.Y;
        /*
        double newX = reflectMatrix[0][0] * point.X + reflectMatrix[1][0] * point.Y +
                      reflectMatrix[2][0];
        double newY = reflectMatrix[0][1] * point.X + reflectMatrix[1][1] * point.Y +
                      reflectMatrix[2][1];
        */
        return new Dot(newX, newY);
    }

    private Dot ScalePoint(Dot point, double[][] scaleMatrix)
    {
        double newX = scaleMatrix[0][0] * point.X + scaleMatrix[1][0] * point.Y +
                      scaleMatrix[2][0];
        double newY = scaleMatrix[0][1] * point.X + scaleMatrix[1][1] * point.Y +
                      scaleMatrix[2][1];
        
        return new Dot(newX, newY);
    }

    private (Dot startPoint, Dot endPoint) GetArcEndPoints(Dot center, double radius, double startAngle,
        double endAngle)
    {
        double x1 = Px(center.X) + Px(radius) * Math.Cos(startAngle * Math.PI / 180);
        double y1 = Px(center.Y) + Px(radius) * Math.Sin(startAngle * Math.PI / 180);
        double x2 = Px(center.X) + Px(radius) * Math.Cos(endAngle * Math.PI / 180);
        double y2 = Px(center.Y) + Px(radius) * Math.Sin(endAngle * Math.PI / 180);
        return (new Dot(x1, y1), new Dot(x2, y2));
    }

    private static double Px(double coord)
    {
        return coord * _pxPerCm;
    }
    
    private static double Cm(double coord)
    {
        return coord / _pxPerCm;
    }

    private void AffineCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        ProjectiveCheckBox.IsChecked = false; // Выключить проективный чекбокс 
    }

    private void AffineCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (ProjectiveCheckBox.IsChecked == false)
        {
            AffineCheckBox.IsChecked = true; // Не позволять оба чекбокса быть выключенными 
        }
    }

    private void ProjectiveCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        AffineCheckBox.IsChecked = false; // Выключить аффинный чекбокс 
    }

    private void ProjectiveCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (AffineCheckBox.IsChecked == false)
        {
            ProjectiveCheckBox.IsChecked = true; // Не позволять оба чекбокса быть выключенными 
        }
    }
}

