using OverpassNet.Entities;
using System.Text;
using System.Xml.Linq;

namespace OverpassNet.Query;

public class OverpassQueryBuilder
{
    internal readonly List<string> QueryBlocks = new();

    /// <summary>
    /// Get area by id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public ElementalQueryBuilder Area(ulong Id)
    {
        QueryBlocks.Add("area");
        QueryBlocks.Add($"({Id});");
        return this.ToElementalBuilder();
    }

    /// <summary>
    /// Get area by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns>First area by popularity with that name</returns>
    public ElementalQueryBuilder Area(string name)
    {
        QueryBlocks.Add("area");
        QueryBlocks.Add($"area('{name}');");
        return this.ToElementalBuilder();
    }

    /// <summary>
    /// Get all ways within a bounding box.
    /// The first or last coordinates outside this bounding box are also produced to allow for properly formed segments 
    /// (this restriction has no effect on derived elements without any geometry).
    /// </summary>
    /// <param name="minLat"></param>
    /// <param name="minLon"></param>
    /// <param name="maxLat"></param>
    /// <param name="maxLon"></param>
    /// <returns></returns>
    public ElementalQueryBuilder Way(double minLat, double minLon, double maxLat, double maxLon)
    {
        QueryBlocks.Add("way");
        QueryBlocks.Add($"({minLat}, {minLon}, {maxLat}, {maxLon});");
        return this.ToElementalBuilder();
    }

    public ElementalQueryBuilder WayByTag(string tag)
    {
        QueryBlocks.Add($"way");
        QueryBlocks.Add($"({tag});");
        return this.ToElementalBuilder();
    }

    /// <summary>
    /// Get the relation by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RelationQueryBuilder Relation(ulong id)
    {
        QueryBlocks.Add("relation"); 
        QueryBlocks.Add($"({id});");
        return this.ToRelationBuilder();
    }

    public OverpassQueryBuilder BeginUnion()
    {
        QueryBlocks.Add("(");
        return this;
    }

    public OverpassQueryBuilder EndUnion()
    {
        QueryBlocks.Add(");");
        return this;
    }

    /// <summary>
    /// Output the result set
    /// </summary>
    /// <returns></returns>
    public OverpassQueryBuilder Output()
    {
        //qt output is by geo area and is much faster than by id
        QueryBlocks.Add("out body;");
        return this;
    }

    /// <summary>
    /// Get all children of the result set.
    /// E.g. All nodes that belong to ways selected
    /// </summary>
    /// <returns></returns>
    public OverpassQueryBuilder RecurseDown()
    {
        QueryBlocks.Add(">;");
        return this;
    }

    public string BuildQuery()
    {
        var query = new StringBuilder("[out:json];");
        foreach (var block in QueryBlocks)
        {
            query.Append(block);
        }
        return query.ToString();
    }

    public async Task<ElementCollection> GetAsync()
    {
        string query = BuildQuery();
        var client = new OverpassClient(new HttpClient());

        var result = await client.Get(query);

        if (result.Elements.Count == 0)
        {
            throw new Exception("No elements found");
        }

        return result;
    }    
}
