using System.Windows.Media;
using System.Windows.Threading;

namespace L2.Core;

public class Animation
{
        private double _a;
        private readonly double _b;
        private double _minA;
        private double _maxA;
        private DispatcherTimer _animationTimer;
        private double _animationRadiusIncrement = 1.0;  // Smaller increment for smoother animation
        private ShapeDrawer _shapeDrawer;
        private CoordinateSystem _coordinateSystem;
        private PascalSnail _pascalSnail;
        private Point _rotationCenter;
        private double _currentT;
        private readonly double _rotationAngle;


        public Animation(PascalSnail pascalSnail, ShapeDrawer shapeDrawer, CoordinateSystem coordinateSystem, Point rotationCenter, double rotationAngle)
        {
            _pascalSnail = pascalSnail;
            _a = pascalSnail.A;
            _b = pascalSnail.B;
            _shapeDrawer = shapeDrawer;
            _coordinateSystem = coordinateSystem;
            _rotationCenter = rotationCenter;
            _rotationAngle = rotationAngle;
        }


        public void StartPointAnimation(PascalSnail pascalSnail, ShapeDrawer shapeDrawer)
        {
            _pascalSnail = pascalSnail;
            _currentT = 0; // Початковий параметр
            _animationTimer = null;

            _animationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10) // Швидкість оновлення
            };
            _animationTimer.Tick += (sender, args) =>
            {
                // Очистити полотно
                _coordinateSystem.DrawCoordinateSystem();
                shapeDrawer.DrawPascalSnail(pascalSnail, _rotationAngle, _rotationCenter.X,
                    _rotationCenter.Y); // Малюємо равлика Паскаля

                // Обчислення поточної точки (x, y)
                double r = pascalSnail.A + pascalSnail.B * Math.Cos(_currentT);
                double x = pascalSnail.CenterX + r * Math.Cos(_currentT);
                double y = pascalSnail.CenterY + r * Math.Sin(_currentT);

                // Малюємо точку
                shapeDrawer.DrawDot(MainWindow.Cm(x), MainWindow.Cm(y), 3, Brushes.Green, 1);

                // Обчислення дотичної
                double dr_dt = -pascalSnail.B * Math.Sin(_currentT); // Похідна r по t
                double dx_dt = dr_dt * Math.Cos(_currentT) - r * Math.Sin(_currentT);
                double dy_dt = dr_dt * Math.Sin(_currentT) + r * Math.Cos(_currentT);

                // Нормалізуємо вектор дотичної
                double tangentLength = Math.Sqrt(dx_dt * dx_dt + dy_dt * dy_dt);
                double tangentX = dx_dt / tangentLength;
                double tangentY = dy_dt / tangentLength;

                // Обчислення нормалі
                double normalX = -tangentY; // Нормаль перпендикулярна до дотичної
                double normalY = tangentX;

                double scaleFactor = 100; // Довжина ліній

                // Малюємо дотичну
                shapeDrawer.DrawLine(
                    MainWindow.Cm(x),
                    MainWindow.Cm(y),
                    MainWindow.Cm(x + tangentX * scaleFactor),
                    MainWindow.Cm(y + tangentY * scaleFactor),
                    Brushes.Red,
                    2);

                shapeDrawer.DrawLine(
                    MainWindow.Cm(x),
                    MainWindow.Cm(y),
                    MainWindow.Cm(x - tangentX * scaleFactor),
                    MainWindow.Cm(y - tangentY * scaleFactor),
                    Brushes.Red,
                    2);

                // Малюємо нормаль
                shapeDrawer.DrawLine(
                    MainWindow.Cm(x),
                    MainWindow.Cm(y),
                    MainWindow.Cm(x + normalX * scaleFactor),
                    MainWindow.Cm(y + normalY * scaleFactor),
                    Brushes.Blue,
                    2);

                shapeDrawer.DrawLine(
                    MainWindow.Cm(x),
                    MainWindow.Cm(y),
                    MainWindow.Cm(x - normalX * scaleFactor),
                    MainWindow.Cm(y - normalY * scaleFactor),
                    Brushes.Blue,
                    2);

                // Оновлюємо t
                _currentT += 0.05; // Крок збільшення параметра t
                if (_currentT > 2 * Math.PI)
                    _currentT = 0; // Повторна анімація
            };

            _animationTimer.Start();
        }



        public void StartAnimation(double minA, double maxA)
        {
            _minA = minA;
            _maxA = maxA;
            _animationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)  // Shorter interval for smoother animation
            };
            _animationTimer.Tick += AnimationStep;
            _animationTimer.Start();
        }
        
        public void StopAnimation()
        {
            if (_animationTimer.IsEnabled)
            {
                _animationTimer.Stop();
            }
        }

        private void AnimationStep(object? sender, EventArgs e)
        {
            // Оновлюємо радіус
            _a += _animationRadiusIncrement;

            // Перевіряємо межі радіуса
            if (_a >= _maxA)
            {
                _a = _maxA; // Забезпечуємо, що не перевищує межу
                _animationRadiusIncrement = -Math.Abs(_animationRadiusIncrement); // Зміна напрямку на спадання
            }
            else if (_a <= _minA)
            {
                _a = _minA; // Забезпечуємо, що не виходить за межу
                _animationRadiusIncrement = Math.Abs(_animationRadiusIncrement); // Зміна напрямку на зростання
            }
            
            _shapeDrawer.DrawAnimatedFigure(new PascalSnail(
                _a,
                _b,
                _pascalSnail.CenterX,
                _pascalSnail.CenterY
            ), _coordinateSystem, _rotationAngle,_rotationCenter);
            
        }
}