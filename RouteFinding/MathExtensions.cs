namespace RouteFinding;
public static class MathExtensions
{
    /// <summary>
    /// Convert degrees to Radians
    /// </summary>
    /// <param name="degrees">Degrees</param>
    /// <returns>The equivalent in radians</returns>
    public static double Radians(this double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
