namespace RouteFinding.Tests;

public class MathExtensionsTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(30, Math.PI / 6)]
    [InlineData(45, Math.PI / 4)]
    [InlineData(60, Math.PI / 3)]
    [InlineData(90, Math.PI / 2)]
    [InlineData(120, 2 * Math.PI / 3)]
    [InlineData(150, 5 * Math.PI / 6)]
    [InlineData(180, Math.PI)]
    [InlineData(210, 7 * Math.PI / 6)]
    [InlineData(360, 2 * Math.PI)]
    public void DegreesToRadians(double degrees, double radians)
    {
        Assert.Equal(radians, degrees.Radians(), 0.0000000000001); //Accurate to 4mm of the earths cercumfrence is ok for floating point precision.
    }
}