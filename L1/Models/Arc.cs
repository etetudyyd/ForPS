using System.Windows.Media;

namespace GMaGD.Models;

public class Arc : Circle
{
    public Point StartPoint { get; set; }
    
    public Point EndPoint { get; set; }
    
    public double StartAngle { get; set; }
    
    public double EndAngle { get; set; }
    
    

    public Arc(double startAngle, double endAngle, Point center, double radius) : base(center, radius)
    {
        StartAngle = startAngle;
        EndAngle = endAngle;
    }
    
}