using OverpassNet.Entities;

namespace RouteFinding;
internal static class ElementCollectionExtensions
{
    //TODO - tests
    public static HashSet<Node> Neighbours(this ElementCollection map, Node current)
    {
        var neighbours = new List<Node>();

        foreach (var way in map.Elements.Where(x => x.Type == ElementType.Way).Cast<Way>())
        {
            if (way.Nodes.Contains(current.UId ?? (ulong)current.Id))
            {
                //Node is in this way
                var index = way.Nodes.ToList().IndexOf(current.UId ?? (ulong)current.Id);

                //TODO - traversing this is slowest of the route generation
                neighbours.Add((Node)map.Elements.FirstOrDefault(x => (ulong)x.Id == way.Nodes.ElementAtOrDefault(index - 1)));
                neighbours.Add((Node)map.Elements.FirstOrDefault(x => (ulong)x.Id == way.Nodes.ElementAtOrDefault(index + 1)));
            }
        }
        return neighbours.Where(x => x != null).ToHashSet();
    }
}
