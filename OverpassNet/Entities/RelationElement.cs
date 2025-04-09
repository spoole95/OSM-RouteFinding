namespace OverpassNet.Entities;

public class RelationElement
{
    public ElementType Type { get; }
    public ulong? Ref { get; set; }
    public string? Role { get; set; }

    public RelationElement(ElementType type)
    {
        Type = type;
    }
}
