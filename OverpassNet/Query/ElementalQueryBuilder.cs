using OverpassNet.Extensions;
using OverpassNet.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverpassNet.Query;
public class ElementalQueryBuilder : OverpassQueryBuilder
{
    internal ElementalQueryBuilder()
    {
    }

    public OverpassQueryBuilder WithTag(string tagType)
    {
        //insert before the last element
        QueryBlocks.Insert(QueryBlocks.Count - 1, $"[\"{tagType}\"]");
        return this;
    }

    public OverpassQueryBuilder WithTag(string tagType, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            //Empty values are not possible by equality, using regular expression
            QueryBlocks.Insert(QueryBlocks.Count - 1, $"[\"{tagType}\"~\"^$\"");
        }
        else
        {
            QueryBlocks.Insert(QueryBlocks.Count - 1, $"[\"{tagType}\"=\"{value}\"]");
        }
        return this;
    }

    public OverpassQueryBuilder WithTags(Dictionary<string, string> tags)
    {
        foreach (var tag in tags)
        {
            WithTag(tag.Key, tag.Value);
        }
        return this;
    }

    public OverpassQueryBuilder WithTags(Dictionary<string, List<string>> tags)
    {
        foreach (var tag in tags)
        {
            foreach (var value in tag.Value)
            {
                WithTag(tag.Key, value);
            }
        }
        return this;
    }

    //public OverpassQueryBuilder WithTag(Highway highwayFlags)
    //{
    //    var tags = highwayFlags.GetIndividualFlags();

    //    foreach(var tag in tags)
    //    {
    //        WithTag("highway", tag);
    //    }

    //    return this;
    //}

}
