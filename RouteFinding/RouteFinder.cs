using OverpassNet.Entities;

namespace RouteFinding;
public static class RouteFinder
{
    public static List<Node>? AStar(Node start, Node goal, ElementCollection map)
    {
        var openList = new List<Node> { start };
        var closedList = new HashSet<Node>();

        // Dictionaries to hold g(n), h(n) and parent pointers
        var gScore = new Dictionary<long, double> { { start.Id, 0 } };
        var hScore = new Dictionary<long, double> { { start.Id, Distance.Between(start, goal) } };
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
                double tentativeGScore = gScore[current.Id] + Distance.Between(current, neighbour);

                if (!gScore.ContainsKey(neighbour.Id) || tentativeGScore < gScore[neighbour.Id])
                {
                    // Update gScore and hScore
                    gScore[neighbour.Id] = tentativeGScore;
                    hScore[neighbour.Id] = Distance.Between(neighbour, goal);

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
