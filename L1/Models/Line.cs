using System.Windows.Controls;
using System.Windows.Media;

namespace GMaGD.Models;

public class Line
{
    public double X1 { get; set; }
    public double Y1 { get; set; }
    public double X2 { get; set; }
    public double Y2 { get; set; }
    public Brush Stroke { get; set; }
    public double StrokeThickness { get; set; } = 1;
    public Point StartPoint { get; set; }
    public Point EndPoint { get; set; }

    public Line(Point startPoint, Point endPoint, Brush stroke, double thickness)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        Stroke = stroke;
        StrokeThickness = thickness;
    }
    
    public Line(double x1, double y1, double x2, double y2, Brush stroke)
    {
        Stroke = stroke;
        StartPoint = new Point(x1, y1);
        EndPoint = new Point(x2, y2);
    }
    
    public void Draw(Canvas canvas)
    {
        // Створюємо лінію з параметрами
        var line = new System.Windows.Shapes.Line
        {
            X1 = StartPoint.X, // Використовуємо координати точок
            Y1 = StartPoint.Y,
            X2 = EndPoint.X,
            Y2 = EndPoint.Y,
            Stroke = this.Stroke,
            StrokeThickness = this.StrokeThickness
        };

        // Додаємо лінію на полотно
        canvas.Children.Add(line);
    }

    
}
