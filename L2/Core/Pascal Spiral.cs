using System.Windows.Media;

namespace L2.Core;

public class PascalSnail(
    double a,
    double b,
    double centerX,
    double centerY
    )
{
    // Властивості, що описують равлика Паскаля
    public int NumLines { get; set; } = 500;
    public double A { get; set; } = a; // Параметр a
    public double B { get; set; } = b; // Параметр b
    public double CenterX { get; set; } = centerX;
    public double CenterY { get; set; } = centerY;
    public Brush Color { get; set; } = Brushes.Black; // За замовчуванням чорний колір
    public double Thickness { get; set; } = 2;
}