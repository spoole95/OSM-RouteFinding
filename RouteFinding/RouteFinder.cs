using OverpassNet.Entities;

namespace RouteFinding;
public class RouteFinder
{
    /// <summary>
    /// Determines the cost to get from the current node to the goal node.
    /// This could have many implementations
    /// </summary>
    /// <param name="current"></param>
    /// <param name="goal"></param>
    /// <returns>Distance in km</returns>
    private static double DistanceBetweenPlaces(Node current, Node goal)
    {
        double dlon = Math.Abs(current.Lon - goal.Lon).Radians();
        double dlat = Math.Abs(current.Lat - goal.Lat).Radians();

        double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(current.Lat.Radians()) * Math.Cos(goal.Lat.Radians()) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
        double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return angle * EARTH_RADIUS;
    }

    /// <summary>
    /// The radius of the earth in kilometers
    /// </summary>
    const double EARTH_RADIUS = 6378.16;



    public static List<Node>? AStar(Node start, Node goal, ElementCollection map)
    {
        var openList = new List<Node> { start };
        var closedList = new HashSet<Node>();

        // Dictionaries to hold g(n), h(n) and parent pointers
        var gScore = new Dictionary<long, double> { { start.Id, 0 } };
        var hScore = new Dictionary<long, double> { { start.Id, DistanceBetweenPlaces(start, goal) } };
        var parentMap = new Dictionary<long, Node>();

        //While there is nodes we can explore
        while (openList.Count > 0)
        {
            //Find the node in the open list with the lowest F score
            var current = openList.OrderBy(x => gScore[x.Id] + hScore[x.Id]).First();

            if (current.Id == goal.Id)
            {
                //We have reached the goal, reconstruct the path
                return ReconstructPath(parentMap, current);
            }

            //We are at the current node, remove it from the open list
            openList.Remove(current);
            closedList.Add(current);

            foreach(var neighbour in map.Neighbours(current))
            {
                if (closedList.Contains(neighbour))
                {
                    //We have already explored this node
                    continue;
                }

                // Tentative gScore (current gScore + distance to neighbor)
                double tentativeGScore = gScore[current.Id] + DistanceBetweenPlaces(current, neighbour);

                if (!gScore.ContainsKey(neighbour.Id) || tentativeGScore < gScore[neighbour.Id])
                {
                    // Update gScore and hScore
                    gScore[neighbour.Id] = tentativeGScore;
                    hScore[neighbour.Id] = DistanceBetweenPlaces(neighbour, goal);

                    // Set the current node as the parent of the neighbor
                    parentMap[neighbour.Id] = current;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return null; // No path found
    }

    private static List<Node> ReconstructPath(Dictionary<long, Node> parentMap, Node current)
    {
        var path = new List<Node> { current };

        while (parentMap.ContainsKey(current.Id))
        {
            current = parentMap[current.Id];
            path.Add(current);
        }

        path.Reverse();
        return path;
    }
}
