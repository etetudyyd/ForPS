using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace L2.Core
{
    public class ShapeDrawer
    {
        private readonly Canvas _canvas;

        public ShapeDrawer(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void DrawLine(double x1, double y1, double x2, double y2, Brush color, double thickness)
        {
            var wpfLine = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = color,
                StrokeThickness = thickness
            };
            _canvas.Children.Add(wpfLine);
        }

        public void DrawPascalSnail(PascalSnail shape, double rotationAngle, double rotationCenterX, double rotationCenterY)
        {
            // Перетворюємо кут обертання у радіани
            double radians = rotationAngle * Math.PI / 180;

            // Початкові координати
            double theta1 = 0;

            double x1 = shape.CenterX + (shape.A + shape.B * Math.Cos(theta1)) * Math.Cos(theta1);
            double y1 = shape.CenterY + (shape.A + shape.B * Math.Cos(theta1)) * Math.Sin(theta1);

            // Малюємо лінії
            for (int i = 0; i < shape.NumLines; i++)
            {
                // Збільшуємо кут
                var theta2 = theta1 + 0.1;

                // Обчислюємо наступну точку равлика
                double x2 = shape.CenterX + (shape.A + shape.B * Math.Cos(theta2)) * Math.Cos(theta2);
                double y2 = shape.CenterY + (shape.A + shape.B * Math.Cos(theta2)) * Math.Sin(theta2);

                // Обертаємо точки (x1, y1) і (x2, y2) навколо точки (rotationCenterX, rotationCenterY)
                double rotatedX1 = rotationCenterX + (x1 - rotationCenterX) * Math.Cos(radians) - (y1 - rotationCenterY) * Math.Sin(radians);
                double rotatedY1 = rotationCenterY + (x1 - rotationCenterX) * Math.Sin(radians) + (y1 - rotationCenterY) * Math.Cos(radians);

                double rotatedX2 = rotationCenterX + (x2 - rotationCenterX) * Math.Cos(radians) - (y2 - rotationCenterY) * Math.Sin(radians);
                double rotatedY2 = rotationCenterY + (x2 - rotationCenterX) * Math.Sin(radians) + (y2 - rotationCenterY) * Math.Cos(radians);

                // Малюємо лінію
                DrawLine(MainWindow.Cm(rotatedX1), MainWindow.Cm(rotatedY1), MainWindow.Cm(rotatedX2), MainWindow.Cm(rotatedY2), shape.Color, shape.Thickness);

                // Оновлюємо координати для наступної ітерації
                x1 = x2;
                y1 = y2;
                theta1 = theta2;
            }
        }


        public void DrawAnimatedFigure(PascalSnail shape, CoordinateSystem coordinateSystem, double rotationAngle, Point rotationCenter)
        {
            // Clear the canvas for each frame to prevent overlapping
            ClearCanvas();

            // Draw the coordinate system background
            coordinateSystem.DrawCoordinateSystem();

            // Draw the updated Astroid shape
            DrawPascalSnail(shape,rotationAngle, rotationCenter.X, rotationCenter.Y);
        }
        


        public void DrawDot(double x, double y, double radius, Brush color, double thickness)
        {
            Circle circle = new Circle(new Point(x, y), radius);
            
            for (int i = 0; i < 360; ++i)
            {
                double rad1 = DegToRad(i);
                double rad2 = DegToRad(i + 1);
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
        
        private double DegToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
        
        public void ClearCanvas()
        {
            _canvas.Children.Clear();
        }
    }
}