namespace L2.Core;

public class Circle
{
    public Point Center { get; set; }
    
    public double Radius { get; set; }

    public Circle(Point center, double radius)
    {
        Center = center;
        Radius = radius;
    }
}