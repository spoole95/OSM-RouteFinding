using OverpassNet.Entities;

namespace RouteFinding;
internal static class Distance
{
    /// <summary>
    /// The radius of the earth in kilometers
    /// </summary>
    private const double EARTH_RADIUS = 6378.16;

    /// <summary>
    /// Determines the cost to get from the current node to the goal node.
    /// This could have many implementations
    /// </summary>
    /// <param name="current"></param>
    /// <param name="goal"></param>
    /// <returns>Distance in km</returns>
    public static double Between(Node current, Node goal)
    {
        double dlon = Math.Abs(current.Lon - goal.Lon).Radians();
        double dlat = Math.Abs(current.Lat - goal.Lat).Radians();

        double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(current.Lat.Radians()) * Math.Cos(goal.Lat.Radians()) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
        double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return angle * EARTH_RADIUS;
    }
}
