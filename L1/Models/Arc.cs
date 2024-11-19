using System.Windows.Media;

namespace GMaGD.Models;

public class Arc : Circle
{
    public Dot StartDot { get; set; }
    
    public Dot EndDot { get; set; }
    
    public double StartAngle { get; set; }
    
    public double EndAngle { get; set; }
    
    

    public Arc(double startAngle, double endAngle, Dot center, double radius) : base(center, radius)
    {
        StartAngle = startAngle;
        EndAngle = endAngle;
    }
    
}