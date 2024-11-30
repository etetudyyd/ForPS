using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using L2.Core;
using Point = L2.Core.Point;

namespace L2
{
    public partial class MainWindow : Window
    {
        private CoordinateSystem _coordinateSystem;
        private ShapeDrawer _shapeDrawer;
        private Animation? _animation;

        public static double PxPerCm { get; set; } = 5;
        public double CenterX { get; set; } = 0;
        public double CenterY { get; set; } = 0;
        private double RotateX { get; set; }
        private double RotateY { get; set; }
        private double RotationAngle { get; set; }
        private double A { get; set; } = 40;
        private double B { get; set; } = 100;
        private double MinA { get; set; } = 20;
        private double MaxA { get; set; } = 90;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _shapeDrawer = new ShapeDrawer(DrawingCanvas);
            _coordinateSystem = new CoordinateSystem(DrawingCanvas, _shapeDrawer);

            _coordinateSystem.ApplyCoordinateSystemTransformation();
            _coordinateSystem.DrawCoordinateSystem();
            DrawFigure();
        }

        private void DrawFigure()
        {
            Point center = new Point(CenterX, CenterY);
            Point rotateCenter = new Point(RotateX, RotateY);
            _shapeDrawer.DrawDot(Cm(rotateCenter.X), Cm(rotateCenter.Y), 4, Brushes.Red, 1);
            CenterX = center.X;
            CenterY = center.Y;
            PascalSnail shape = new PascalSnail(A, B, CenterX, CenterY);
            _shapeDrawer.DrawPascalSnail(shape, RotationAngle, RotateX, RotateY);
            CalculationsPascalSnail(shape);
            GetData();
        }

        
        
        
        public void CalculationsPascalSnail(PascalSnail shape)
        {
            double arcLength = Calculations.CalculateArcLength(A, B, 0, Math.PI);
            ArcLengthText.Text = arcLength.ToString("F1");
            
            double area = Calculations.CalculateArea(A, B);
            AreaText.Text = area.ToString("F1");

            double radiusOfCurvature = Calculations.CalculateRadiusOfCurvature(A, B, Math.PI / 4);
            RadiusOfCurvatureText.Text = radiusOfCurvature.ToString("F1");

        }
        
        private void ApplyRotateParameters_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            double.TryParse(RotateXInput.Text, out double rotateX);
            double.TryParse(RotateYInput.Text, out double rotateY);
            double.TryParse(RotateAngleInput.Text, out double rotationAngle);

            RotateX = rotateX;
            RotateY = rotateY;
            RotationAngle = rotationAngle;

            _coordinateSystem.DrawCoordinateSystem();
            DrawFigure();

        }

        private void ApplySettingsPxPerCm_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(PxPerCmInput.Text, out double pxPerCm))
            {
                PxPerCm = pxPerCm;
                _coordinateSystem.PxPerCm = pxPerCm;

                _coordinateSystem.DrawCoordinateSystem();
                DrawFigure();
            }
        }

        private void ApplyParameters_Click(object sender, RoutedEventArgs e)
        {
            double.TryParse(CenterXInput.Text, out double centerX);
            double.TryParse(CenterYInput.Text, out double centerY);
            double.TryParse(AInput.Text, out double a);
            double.TryParse(BInput.Text, out double b);

            CenterX = centerX;
            CenterY = centerY;
            A = a;
            B = b;

            _coordinateSystem.DrawCoordinateSystem();
            DrawFigure();
        }

        private void GetData()
        {
            PxPerCmInput.Text = PxPerCm.ToString(CultureInfo.InvariantCulture);
            CenterXInput.Text = CenterX.ToString(CultureInfo.InvariantCulture);
            CenterYInput.Text = CenterY.ToString(CultureInfo.InvariantCulture);
            AInput.Text = A.ToString(CultureInfo.InvariantCulture);
            BInput.Text = B.ToString(CultureInfo.InvariantCulture);

            RotateXInput.Text = RotateX.ToString(CultureInfo.InvariantCulture);
            RotateYInput.Text = RotateY.ToString(CultureInfo.InvariantCulture);
            RotateAngleInput.Text = RotationAngle.ToString(CultureInfo.InvariantCulture);

            MinAInput.Text = MinA.ToString(CultureInfo.InvariantCulture);
            MaxAInput.Text = MaxA.ToString(CultureInfo.InvariantCulture);
        }

        private void ApplyClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            _shapeDrawer.ClearCanvas();
            _coordinateSystem.DrawCoordinateSystem();
        }

        private void StopAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            _animation?.StopAnimation();
        }


        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(DrawingCanvas);
            double xCoordCm = mousePosition.X / PxPerCm;
            double yCoordCm = mousePosition.Y / PxPerCm;
            Title =
                $"X/px: {mousePosition.X:0.00}, Y/px: {mousePosition.Y:0.00} X/cm: {xCoordCm:0.00}, Y/cm: {yCoordCm:0.00}";
        }

        private void ApplyAnimationRadius_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(MinAInput.Text, out double minA) &&
                double.TryParse(MaxAInput.Text, out double maxA))
            {
                MinA = minA;
                MaxA = maxA;
                _animation = new Animation(
                    new PascalSnail(A, B, Cm(CenterX), Cm(CenterY)),
                    _shapeDrawer, _coordinateSystem, new Point(RotateX, RotateY), RotationAngle);
                _animation.StartAnimation(minA, maxA);
            }
        }

        public static double Cm(double coord)
        {
            return coord * PxPerCm;
        }

        private void StartAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            _animation = new Animation(new PascalSnail(A, B, CenterX, CenterY), _shapeDrawer, _coordinateSystem,
                new Point(RotateX, RotateY), RotationAngle);
            _animation?.StartPointAnimation(new PascalSnail(A, B, CenterX, CenterY), _shapeDrawer);
        }
    }
}