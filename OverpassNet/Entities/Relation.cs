namespace OverpassNet.Entities;

public class Relation(long id) : Element(ElementType.Relation, id)
{
    public IReadOnlyCollection<RelationElement>? Members { get; set; }
}
