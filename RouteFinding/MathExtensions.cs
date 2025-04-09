namespace RouteFinding;
internal static class MathExtensions
{
    /// <summary>
    /// Convert degrees to Radians
    /// </summary>
    /// <param name="x">Degrees</param>
    /// <returns>The equivalent in radians</returns>
    public static double Radians(this double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
