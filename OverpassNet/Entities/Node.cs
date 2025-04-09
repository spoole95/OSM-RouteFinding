namespace OverpassNet.Entities;

public class Node(long id) : Element(ElementType.Node, id)
{
    public double Lon { get; set; }
    public double Lat { get; set; }
}
