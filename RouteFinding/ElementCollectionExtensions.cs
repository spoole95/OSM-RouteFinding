using OverpassNet.Entities;

namespace RouteFinding;
internal static class ElementCollectionExtensions
{
    //TODO - tests
    public static HashSet<Node> Neighbours(this ElementCollection map, Node current)
    {
        var neighbours = new List<Node>();

        foreach (var way in map.Ways.Values)
        {
            if (way.Nodes.Contains(current.UId ?? (ulong)current.Id))
            {
                //Node is in this way
                var index = way.Nodes.ToList().IndexOf(current.UId ?? (ulong)current.Id);

                if( map.Nodes.TryGetValue(way.Nodes.ElementAtOrDefault(index - 1), out var n1))
                {
                    neighbours.Add(n1);
                }
                if( map.Nodes.TryGetValue(way.Nodes.ElementAtOrDefault(index + 1), out var n2))
                {
                    neighbours.Add(n2);
                }
            }
        }
        return neighbours.Where(x => x != null).ToHashSet();
    }
}
