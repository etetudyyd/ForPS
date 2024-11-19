namespace GMaGD.Models;

public class Circle
{
    public Dot Center { get; set; }
    
    public double Radius { get; set; }

    public Circle(Dot center, double radius)
    {
        Center = center;
        Radius = radius;
    }
    
   
}