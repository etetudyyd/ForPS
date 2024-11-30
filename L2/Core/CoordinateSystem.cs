// CoordinateSystem.cs
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace L2.Core
{
    public class CoordinateSystem
    {
        private readonly Canvas _canvas;
        private readonly ShapeDrawer _shapeDrawer;
        public double PxPerCm { get; set; } = 5;
        private const double LineThickness = 1;
        private const double AxisThickness = 3;
        private const double GridLength = 500;

        public CoordinateSystem(Canvas canvas, ShapeDrawer shapeDrawer)
        {
            _canvas = canvas;
            _shapeDrawer = shapeDrawer;
        }

        public void ApplyCoordinateSystemTransformation()
        {
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(1, -1));
            transformGroup.Children.Add(new TranslateTransform(_canvas.ActualWidth / 2, _canvas.ActualHeight / 2));
            _canvas.RenderTransform = transformGroup;
        }

        public void DrawCoordinateSystem()
        {
            _canvas.Children.Clear();

            double centerX = _canvas.ActualWidth / 2;
            double centerY = _canvas.ActualHeight / 2;
            double gridScale = 10;
            double gridSize = PxPerCm * gridScale;

            for (double i = -GridLength; i <= GridLength; i++)
            {
                _shapeDrawer.DrawLine(-centerX, i * gridSize, GridLength * gridSize, i * gridSize, Brushes.LightGray, LineThickness);
                _shapeDrawer.DrawLine(i * gridSize, -centerY, i * gridSize, GridLength * gridSize, Brushes.LightGray, LineThickness);
            }

            _shapeDrawer.DrawLine(-centerX, 0, GridLength * gridSize, 0, Brushes.LimeGreen, AxisThickness);
            _shapeDrawer.DrawLine(0, -centerY, 0, GridLength * gridSize, Brushes.Red, AxisThickness);
        }
    }
}