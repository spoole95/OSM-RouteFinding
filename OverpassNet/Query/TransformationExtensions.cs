namespace OverpassNet.Query;
internal static class TransformationExtensions
{
    public static ElementalQueryBuilder ToElementalBuilder(this OverpassQueryBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        // Create a new instance of ElementalQueryBuilder
        var elementalBuilder = new ElementalQueryBuilder();

        // Copy the state from the OverpassQueryBuilder
        elementalBuilder.QueryBlocks.AddRange(builder.QueryBlocks);

        return elementalBuilder;
    }

    public static RelationQueryBuilder ToRelationBuilder(this OverpassQueryBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        // Create a new instance of RelationQueryBuilder
        var relationBuilder = new RelationQueryBuilder();

        // Copy the state from the OverpassQueryBuilder
        relationBuilder.QueryBlocks.AddRange(builder.QueryBlocks);

        return relationBuilder;
    }
}
