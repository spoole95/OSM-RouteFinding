namespace OverpassNet.Query;
public class RelationQueryBuilder : ElementalQueryBuilder
{
    internal RelationQueryBuilder()
    {
    }

    public RelationQueryBuilder ToArea(string key)
    {
        if (!key.StartsWith('.'))
        {
            key = "." + key;
        }

        QueryBlocks.Add($"map_to_area -> {key};");
        return this;
    }
}
