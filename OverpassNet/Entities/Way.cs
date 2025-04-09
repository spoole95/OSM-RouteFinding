namespace OverpassNet.Entities;

public class Way : Element
{
    public ulong[] Nodes { get; set; }

    public Way(long id) : base(ElementType.Way, id)
    {
    }
}
