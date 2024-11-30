using MathNet.Numerics.Integration;

namespace L2.Core;

public class Calculations
{
    
    public static double CalculateArcLength(double a, double b, double theta1, double theta2, int numSteps = 1000)
    {
        double step = (theta2 - theta1) / numSteps;
        double arcLength = 0.0;

        for (int i = 0; i < numSteps; i++)
        {
            double theta = theta1 + i * step;
            double r = a + b * Math.Cos(theta);
            double dr = -b * Math.Sin(theta);

            double segmentLength = Math.Sqrt(r * r + dr * dr) * step;
            arcLength += segmentLength;
        }

        return arcLength;
    }

    public static double CalculateArea(double a, double b)
    {
        return Math.PI * (a * a + (b * b) / 2);
    }
    

    public static double CalculateRadiusOfCurvature(double a, double b, double theta)
    {
        // Радіус кривої
        double r = a + b * Math.Cos(theta);
    
        // Перша похідна
        double dr = -b * Math.Sin(theta);
    
        // Друга похідна
        double d2r = -b * Math.Cos(theta);
    
        // Чисельник формули для радіусу кривизни
        double numerator = Math.Pow(r * r + dr * dr, 1.5);
    
        // Знаменник формули для радіусу кривизни
        double denominator = Math.Abs(r * d2r - 2 * dr * dr + r);
    
        // Обчислення та повернення радіусу кривизни
        return numerator / denominator;
    }


}