namespace OverpassNet.Entities;

public class Way : Element
{
    public ulong[] Nodes { get; set; }

    public Way(long id) : base(ElementType.Way, id)
    {
    }

    //TODO - use tags to get way type (path, road, canal, etc)
}
