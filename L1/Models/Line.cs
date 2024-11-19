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
    public Dot StartDot { get; set; }
    public Dot EndDot { get; set; }

    public Line(Dot startDot, Dot endDot, Brush stroke, double thickness)
    {
        StartDot = startDot;
        EndDot = endDot;
        Stroke = stroke;
        StrokeThickness = thickness;
    }
    
    public Line(double x1, double y1, double x2, double y2, Brush stroke)
    {
        Stroke = stroke;
        StartDot = new Dot(x1, y1);
        EndDot = new Dot(x2, y2);
    }
    
    public void Draw(Canvas canvas)
    {
        // Створюємо лінію з параметрами
        var line = new System.Windows.Shapes.Line
        {
            X1 = StartDot.X, // Використовуємо координати точок
            Y1 = StartDot.Y,
            X2 = EndDot.X,
            Y2 = EndDot.Y,
            Stroke = this.Stroke,
            StrokeThickness = this.StrokeThickness
        };

        // Додаємо лінію на полотно
        canvas.Children.Add(line);
    }

    
}
