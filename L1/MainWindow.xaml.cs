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
using Point = GMaGD.Models.Point;

namespace GMaGD;

public partial class MainWindow : Window
{
    private static double _pxPerCm = 10;
    private const double LineThickness = 1;
    private const double CoordLineThickness = 0.25;
    private double AxisThickness = 3;
    private double rotationAngle;
    private double rotationX = 50;
    private double rotationY = 50;
    private double locationX = 50;
    private double locationY = 50;
    
    private double Sx = 1;
    private double Sy = 1;
    private double X = 75;
    private double Y = 75;
    private double xX = 1;
    private double yX = 0;
    private double xY = 0;
    private double yY = 1;
    private double x0 = 0;
    private double y0 = 0;
    private double xXw = 1500;
    private double xYw = 0;
    private double wX = 1;
    private double yXw = 0;
    private double yYw = 1500;
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

    private double dX;
    private double dY;
    
    public MainWindow()
    {
        InitializeComponent();
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
            [2*X, 2*Y, 1]
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
        
        
        
         Point center = new Point(locationX + x0, locationY + y0);
        Point rotateCenter = new Point(rotationX, rotationY);
        
       
       
        DrawPoint(Px(rotateCenter.X), Px(rotateCenter.Y), _pxPerCm/3, Brushes.Blue, LineThickness);
        
        center = pointEuclidRotation(center, rotateCenter, rotationAngle);

        
        
        Arc smallArc = new Arc(90, 270, center, _smallArcRadius);
        Arc largeArc = new Arc(134.5, 224.5, center, _largeArcRadius);
        
        
        DrawPoint(Px(center.X), Px(center.Y), _pxPerCm/3, Brushes.Black, LineThickness);
        
        var fgDots = GetArcEndPoints(smallArc.Center, smallArc.Radius, smallArc.StartAngle, smallArc.EndAngle);
        var laDots = GetArcEndPoints(largeArc.Center, largeArc.Radius, largeArc.StartAngle, largeArc.EndAngle+1);

        Point A = new Point(Cm(laDots.startPoint.X), Cm(laDots.startPoint.Y));
        Point L = new Point(Cm(laDots.endPoint.X), Cm(laDots.endPoint.Y));
        Point G = new Point(Cm(fgDots.endPoint.X), Cm(fgDots.endPoint.Y));
        Point F = new Point(Cm(fgDots.startPoint.X),Cm(fgDots.startPoint.Y));
        
        Point E = new Point(F.X + (Ef), F.Y);
        Point H = new Point(G.X + (Gh), G.Y);
        Point D = new Point(E.X, E.Y - (De));
        Point I = new Point(H.X, H.Y + (Hi));
        Point C = new Point(D.X + (Cd), D.Y);
        Point J = new Point(I.X + (Ij), I.Y);
        Point B = new Point(C.X  + Ab - 15, C.Y + (Bc));
        Point K = new Point(J.X + Kl - 15, J.Y - (Jk));
        
        A = pointEuclidRotation(A, center, rotationAngle);
        L = pointEuclidRotation(L, center, rotationAngle);
        G = pointEuclidRotation(G, center, rotationAngle);
        F = pointEuclidRotation(F, center, rotationAngle);
        
        E = pointEuclidRotation(E, center, rotationAngle);
        H = pointEuclidRotation(H, center, rotationAngle);
        
        D = pointEuclidRotation(D, center, rotationAngle);
        I = pointEuclidRotation(I, center, rotationAngle);
        C = pointEuclidRotation(C, center, rotationAngle);
        J = pointEuclidRotation(J, center, rotationAngle);
        B = pointEuclidRotation(B, center, rotationAngle);
        K = pointEuclidRotation(K, center, rotationAngle);
       
        
        if (ReflectionCheckBox.IsChecked == true)
        {
            center = ReflectPoint(center, reflectMatrix);
        }
        
        smallArc.Center = center;
         largeArc.Center = center;
        
        DrawRotatedArc(smallArc, rotationAngle, smallArc.Radius, matrixAffine, matrixProjective, scaleMatrix);
        DrawRotatedArc(largeArc, rotationAngle, largeArc.Radius, matrixAffine, matrixProjective, scaleMatrix);
        
        if (ReflectionCheckBox.IsChecked == true)
        {
            A = ReflectPoint(A, reflectMatrix);
            L = ReflectPoint(L, reflectMatrix);
            G = ReflectPoint(G, reflectMatrix);
            F = ReflectPoint(F, reflectMatrix);
            E = ReflectPoint(E, reflectMatrix);
            H = ReflectPoint(H, reflectMatrix);
            D = ReflectPoint(D, reflectMatrix);
            I = ReflectPoint(I, reflectMatrix);
            C = ReflectPoint(C, reflectMatrix);
            J = ReflectPoint(J, reflectMatrix);
            B = ReflectPoint(B, reflectMatrix);
            K = ReflectPoint(K, reflectMatrix);
        }
        
        DrawPoint(Px(center.X), Px(center.Y), _pxPerCm/3, Brushes.Black, LineThickness);
        
        
        if (AffineCheckBox.IsChecked == true || (AffineCheckBox.IsChecked == false && ProjectiveCheckBox.IsChecked == false))
        {
            A = AffineTransformation(new Point(Px(A.X), Px(A.Y)), matrixAffine);
            L = AffineTransformation(new Point(Px(L.X), Px(L.Y)), matrixAffine);
            G = AffineTransformation(new Point(Px(G.X), Px(G.Y)), matrixAffine);
            F = AffineTransformation(new Point(Px(F.X), Px(F.Y)), matrixAffine);
            E = AffineTransformation(new Point(Px(E.X), Px(E.Y)), matrixAffine);
            H = AffineTransformation(new Point(Px(H.X), Px(H.Y)), matrixAffine);
            D = AffineTransformation(new Point(Px(D.X), Px(D.Y)), matrixAffine);
            I = AffineTransformation(new Point(Px(I.X), Px(I.Y)), matrixAffine);
            C = AffineTransformation(new Point(Px(C.X), Px(C.Y)), matrixAffine);
            J = AffineTransformation(new Point(Px(J.X), Px(J.Y)), matrixAffine);
            B = AffineTransformation(new Point(Px(B.X), Px(B.Y)), matrixAffine);
            K = AffineTransformation(new Point(Px(K.X), Px(K.Y)), matrixAffine);
        }
        else
        {
            A = ProjectiveTransformation(new Point(Px(A.X), Px(A.Y)), matrixProjective);
            L = ProjectiveTransformation(new Point(Px(L.X), Px(L.Y)), matrixProjective);
            G = ProjectiveTransformation(new Point(Px(G.X), Px(G.Y)), matrixProjective);
            F = ProjectiveTransformation(new Point(Px(F.X), Px(F.Y)), matrixProjective);
            E = ProjectiveTransformation(new Point(Px(E.X), Px(E.Y)), matrixProjective);
            H = ProjectiveTransformation(new Point(Px(H.X), Px(H.Y)), matrixProjective);
            D = ProjectiveTransformation(new Point(Px(D.X), Px(D.Y)), matrixProjective);
            I = ProjectiveTransformation(new Point(Px(I.X), Px(I.Y)), matrixProjective);
            C = ProjectiveTransformation(new Point(Px(C.X), Px(C.Y)), matrixProjective);
            J = ProjectiveTransformation(new Point(Px(J.X), Px(J.Y)), matrixProjective);
            B = ProjectiveTransformation(new Point(Px(B.X), Px(B.Y)), matrixProjective);
            K = ProjectiveTransformation(new Point(Px(K.X), Px(K.Y)), matrixProjective);
        }
      
        A = ScalePoint(A, scaleMatrix);
        L = ScalePoint(L, scaleMatrix);
        G = ScalePoint(G, scaleMatrix);
        F = ScalePoint(F, scaleMatrix);
        E = ScalePoint(E, scaleMatrix);
        H = ScalePoint(H, scaleMatrix);
        D = ScalePoint(D, scaleMatrix);
        I = ScalePoint(I, scaleMatrix);
        C = ScalePoint(C, scaleMatrix);
        J = ScalePoint(J, scaleMatrix);
        B = ScalePoint(B, scaleMatrix);
        K = ScalePoint(K, scaleMatrix);
        
        
        DrawLine((C.X), (C.Y), (B.X), (B.Y),Brushes.Black, LineThickness);
        DrawLine((J.X), (J.Y), (K.X), (K.Y),Brushes.Black, LineThickness);
        
        DrawLine((D.X), (D.Y), (E.X), (E.Y), Brushes.Black, LineThickness);
        DrawLine((I.X), (I.Y), (H.X), (H.Y), Brushes.Black, LineThickness);
        
        DrawLine((D.X), (D.Y), (C.X), (C.Y), Brushes.Black, LineThickness);
        DrawLine((I.X), (I.Y), (J.X), (J.Y), Brushes.Black, LineThickness);
        
        DrawLine((F.X), (F.Y), (E.X), (E.Y), Brushes.Black, LineThickness);
        DrawLine((G.X), (G.Y), (H.X), (H.Y), Brushes.Black, LineThickness);
       
        DrawLine((K.X), (K.Y), (L.X), (L.Y) , Brushes.Black, LineThickness);
        DrawLine((B.X), (B.Y), (A.X), (A.Y) , Brushes.Black, LineThickness);
        
       
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
        double.TryParse(RotationAngleInput.Text, out rotationAngle);
        double.TryParse(RotationXInput.Text, out rotationX);
        double.TryParse(RotationYInput.Text, out rotationY);
        DrawCoordinateSystem();
        DrawShape();
    }

    private void ApplyLocation_Click(object sender, RoutedEventArgs e)
    {
        double.TryParse(LocationXInput.Text, out locationX);
        double.TryParse(LocationYInput.Text, out locationY);
        DrawCoordinateSystem();
        DrawShape();
    }

    private void ApplySettingsPxPerCm_Click(object sender, RoutedEventArgs e)
    {
        double.TryParse(PxPerCmInput.Text, out _pxPerCm);
        DrawCoordinateSystem();
        DrawShape();
    }

    private void ApplyAffineTransformation_Click(object sender, RoutedEventArgs e)
    {
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
        double.TryParse(ReflectionXInput.Text, out X);
        double.TryParse(ReflectionYInput.Text, out Y);
        double.TryParse(ReflectiondXInput.Text, out double dX);
        double.TryParse(ReflectiondYInput.Text, out double dY);
        
        DrawCoordinateSystem();
        DrawShape();
    }
    
    private void ApplyScaling_Click(object sender, RoutedEventArgs e)
    {
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
        System.Windows.Point mousePosition = e.GetPosition(DrawingCanvas);
        double xCoord = mousePosition.X ;
        double yCoord = mousePosition.Y;
        double xCoordCm = xCoord / _pxPerCm;
        double yCoordCm = yCoord / _pxPerCm;
        Title = $"X/px: {xCoord:0.00}, Y/px: {yCoord:0.00} X/cm: {xCoordCm:0.00}, Y/cm: {yCoordCm:0.00} ";
    }

// Метод для малювання лінії на Canvas 
    private void DrawLine(double x1, double y1, double x2, double y2, Brush color, double thickness)
    {
        var customLine = new Line(x1, y1, x2, y2, color);
        var wpfLine = new System.Windows.Shapes.Line()
        {
            X1 = customLine.StartPoint.X,
            Y1 = customLine.StartPoint.Y,
            X2 = customLine.EndPoint.X,
            Y2 = customLine.EndPoint.Y,
            Stroke = color,
            StrokeThickness = thickness
        };
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
        
        double[][] matrixAffine =
        [
            [xX, yX, 0],
            [xY, yY, 0],
            [x0, y0, 1]
        ];

        double width = DrawingCanvas.ActualWidth;
        double height = DrawingCanvas.ActualHeight;

        if (AffineCheckBox.IsChecked == true ||
            (AffineCheckBox.IsChecked == false && ProjectiveCheckBox.IsChecked == false))
        {
            double gridSize = _pxPerCm;
            double cellCountX = width;
            double cellCountY = height;
            for (int i = 0; i <= cellCountX; i++)
            {
                if (i == 0)
                {
                    DrawAffineLine(matrixAffine, x0, y0 + i * gridSize, cellCountX * gridSize, i * gridSize,
                        Brushes.LimeGreen, AxisThickness);
                    DrawAffineLine(matrixAffine, i * gridSize+ x0, y0, i * gridSize, cellCountY * gridSize,
                        Brushes.Red, AxisThickness);
                }
                else
                {
                    DrawAffineLine(matrixAffine, x0, y0 + i * gridSize, cellCountX * gridSize, i * gridSize,
                        Brushes.LightGray, LineThickness);
                    DrawAffineLine(matrixAffine, x0 + i * gridSize, y0, i * gridSize, cellCountY * gridSize,
                        Brushes.LightGray, LineThickness);
                }
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

        DrawPoint(Px(x0), Px(y0), Px(2) / _pxPerCm, Brushes.Black, AxisThickness);
        DrawPoint(Px(xX + x0), Px(yX + y0), Px(2 / _pxPerCm), Brushes.LimeGreen, AxisThickness);
        DrawPoint(Px(xY + x0), Px(yY + y0), Px(2 / _pxPerCm), Brushes.Red, AxisThickness);
    }

    private void DrawProjectiveLine(double[][] matrixProjective, double x1, double y1, double x2,
        double y2, Brush color, double thickness)
    {
// Перетворюємо точки лінії за допомогою проективної матриці 
        var p1 = ProjectiveTransformation(new Point(x1, y1), matrixProjective);
        var p2 = ProjectiveTransformation(new Point(x2, y2), matrixProjective);
        var customLine = new Line(p1.X, p1.Y, p2.X, p2.Y, color);
// Створюємо WPF-лінію 
        var wpfLine = new System.Windows.Shapes.Line()
        {
            X1 = customLine.StartPoint.X,
            Y1 = customLine.StartPoint.Y,
            X2 = customLine.EndPoint.X,
            Y2 = customLine.EndPoint.Y,
            Stroke = color,
            StrokeThickness = thickness
        };
// Додаємо лінію до Canvas 
        DrawingCanvas.Children.Add(wpfLine);
    }
    
    private void DrawAffineLine(double[][] matrixAffine, double x1, double y1, double x2,
        double y2, Brush color, double thickness)
    {
// Перетворюємо точки лінії за допомогою проективної матриці 
        var p1 = AffineTransformation(new Point(x1, y1), matrixAffine);
        var p2 = AffineTransformation(new Point(x2, y2), matrixAffine);
        var customLine = new Line(p1.X, p1.Y, p2.X, p2.Y, color);
// Створюємо WPF-лінію 
        var wpfLine = new System.Windows.Shapes.Line()
        {
            X1 = customLine.StartPoint.X,
            Y1 = customLine.StartPoint.Y,
            X2 = customLine.EndPoint.X,
            Y2 = customLine.EndPoint.Y,
            Stroke = color,
            StrokeThickness = thickness
        };
// Додаємо лінію до Canvas 
        DrawingCanvas.Children.Add(wpfLine);
    }

    private void DrawArc(double x, double y, double radius, double startAngle, double endAngle, Brush color,
        double thickness, double[][] matrixAffine, double[][] matrixProjective, double[][] matrixScale)
    {
        Arc arc = new Arc(startAngle, endAngle, new Point(x, y), radius);
        for (double i = startAngle; i <= endAngle;  i += 0.5)
        {
            double rad1 = degToRad(i);
            double rad2 = degToRad(i + 1);

            Point Pbeg;
            Point Pend;
            
            if (ReflectionCheckBox.IsChecked == true)
            {
                Pbeg = new Point(
                    arc.Center.X + arc.Radius * -Math.Cos(rad1),
                    arc.Center.Y - arc.Radius * -Math.Sin(rad1)
                );
                 Pend = new Point(
                    arc.Center.X + arc.Radius * -Math.Cos(rad2),
                    arc.Center.Y - arc.Radius * -Math.Sin(rad2)
                );
            }
            else
            {
                 Pbeg = new Point(
                    arc.Center.X + arc.Radius * Math.Cos(rad1),
                    arc.Center.Y - arc.Radius * Math.Sin(rad1)
                );
                 Pend = new Point(
                    arc.Center.X + arc.Radius * Math.Cos(rad2),
                    arc.Center.Y - arc.Radius * Math.Sin(rad2)
                );
            }
            
            Pbeg = ScalePoint(Pbeg, matrixScale);
            Pend = ScalePoint(Pend, matrixScale);
            
            if (AffineCheckBox.IsChecked == true)
            {
                Pbeg = AffineArcTransformation(Pbeg, matrixAffine);
                Pend = AffineArcTransformation(Pend, matrixAffine);
            }
            else if (ProjectiveCheckBox.IsChecked == true)
            {
                Pbeg = ProjectiveTransformation(Pbeg, matrixProjective);
                Pend = ProjectiveTransformation(Pend, matrixProjective);
            }
            
            DrawLine(Pbeg.X, Pbeg.Y, Pend.X, Pend.Y, color, thickness);
            
        }
    }

    private void DrawPoint(double x, double y, double radius, Brush color, double thickness)
    {
        Circle circle = new Circle(new Point(x, y), radius);
        for (int i = 0; i < 360; ++i)
        {
            double rad1 = degToRad(i);
            double rad2 = degToRad(i + 1);
            Point Pbeg = new Point(
                circle.Center.X + radius * Math.Cos(rad1),
                circle.Center.Y + radius * Math.Sin(rad1)
            );
            Point Pend = new Point(
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

    private Point AffineTransformation(Point point, double[][] transformationMatrix)
    {
        double newX = transformationMatrix[0][0] * point.X + transformationMatrix[1][0] * point.Y +
                      transformationMatrix[2][0] * 1;
        double newY = transformationMatrix[0][1] * point.X + transformationMatrix[1][1] * point.Y +
                      transformationMatrix[2][1] * 1;
        return new Point(newX, newY);
    }
    
    private Point AffineArcTransformation(Point point, double[][] transformationMatrix)
    {
        double newX = transformationMatrix[0][0] * point.X + transformationMatrix[1][0] * point.Y +
                      transformationMatrix[2][0] * 1;
        double newY = transformationMatrix[0][1] * point.X + transformationMatrix[1][1] * point.Y +
                      transformationMatrix[2][1] * 1;
        return new Point(newX, newY);
    }

    public Point ProjectiveTransformation(Point point, double[][] matrixProjective)
    {
        double XPrime = matrixProjective[0][0] * point.X + matrixProjective[1][0] * point.Y + matrixProjective[2][0];
        double YPrime = matrixProjective[0][1] * point.X + matrixProjective[1][1] * point.Y + matrixProjective[2][1];
        double WPrime = matrixProjective[0][2] * point.X + matrixProjective[1][2] * point.Y + matrixProjective[2][2];

        double XResult = XPrime / WPrime;
        double YResult = YPrime / WPrime;
        return new Point(XResult, YResult);
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
        ReflectionXInput.Text = X.ToString();
        ReflectionYInput.Text = Y.ToString();
    }

    private void DrawRotatedArc(Arc arc, double rotationAngle, double radius, double[][] matrixAffine,
        double[][] matrixProjective, double [][] scaleMatrix)
    {
        double adjustedStartAngle = (arc.StartAngle + rotationAngle) % 360;
        double adjustedEndAngle = (arc.EndAngle + rotationAngle) % 360;
        if (adjustedStartAngle < 0) adjustedStartAngle += 360;
        if (adjustedEndAngle < 0) adjustedEndAngle += 360;
        if (adjustedEndAngle < adjustedStartAngle)
        {
            DrawArc(Px(arc.Center.X), Px(arc.Center.Y), Px(radius), adjustedStartAngle, 360,
                Brushes.Black, LineThickness, matrixAffine, matrixProjective, scaleMatrix);
            DrawArc(Px(arc.Center.X), Px(arc.Center.Y), Px(radius), 0, adjustedEndAngle,
                Brushes.Black, LineThickness, matrixAffine, matrixProjective, scaleMatrix);
        }
        else
        {
            DrawArc(Px(arc.Center.X), Px(arc.Center.Y), Px(radius), adjustedStartAngle,
                adjustedEndAngle, Brushes.Black, LineThickness, matrixAffine, matrixProjective, scaleMatrix);
        }
    }

    private void UpdateLineCoordinates(Point startPoint, Point endPoint, Brush color)
    {
        if (!startPoint.Equals(endPoint))
            DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, color, LineThickness);
    }

    private Point pointEuclidRotation(Point point, Point center, double angle)
    {
        double angleInRadians = -angle * Math.PI / 180;
        double cosA = Math.Cos(angleInRadians);
        double sinA = Math.Sin(angleInRadians);
        double newX = center.X + (point.X - center.X) * cosA - (point.Y - center.Y) * sinA;
        double newY = center.Y + (point.X - center.X) * sinA + (point.Y - center.Y) * cosA;
        return new Point(newX, newY);
    }

    private Point ReflectPoint(Point point, double[][] reflectMatrix)
    {
        
        double  newX = 2*X - point.X;
        double  newY = 2*Y - point.Y;
        
        return new Point(newX, newY);
    }

    private Point ScalePoint(Point point, double[][] scaleMatrix)
    {
        double newX = scaleMatrix[0][0] * point.X + scaleMatrix[1][0] * point.Y +
                      scaleMatrix[2][0];
        double newY = scaleMatrix[0][1] * point.X + scaleMatrix[1][1] * point.Y +
                      scaleMatrix[2][1];
        
        return new Point(newX, newY);
    }

    private (Point startPoint, Point endPoint) GetArcEndPoints(Point center, double radius, double startAngle,
        double endAngle)
    {
        double x1 = Px(center.X) + Px(radius) * Math.Cos(startAngle * Math.PI / 180);
        double y1 = Px(center.Y) + Px(radius) * Math.Sin(startAngle * Math.PI / 180);
        double x2 = Px(center.X) + Px(radius) * Math.Cos(endAngle * Math.PI / 180);
        double y2 = Px(center.Y) + Px(radius) * Math.Sin(endAngle * Math.PI / 180);
        return (new Point(x1, y1), new Point(x2, y2));
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

    private void ApplyClearCanvas_Click(object sender, RoutedEventArgs e)
    {
        DrawingCanvas.Children.Clear();
    }
}

