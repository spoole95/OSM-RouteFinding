using OverpassNet.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverpassNet.Tests;
public class OverpassQueryBuilderTests
{
    [Fact]
    public void HighwaysInAnArea_ShouldBuild()
    {
        var query = new OverpassQueryBuilder()
            .Way(52.463465, -0.962827, 52.499166, -0.887756)
            .WithTag("highway")
            .Output()
            .RecurseDown()
            .Output()
            .BuildQuery();

        Assert.Equal("[out:json];way[\"highway\"](52.463465, -0.962827, 52.499166, -0.887756);out body;>;out body;", query);
    }

    [Fact]
    public void CanalsInArea_ShouldBuild()
    {
        var query = new OverpassQueryBuilder()
                .Relation(8485220)
                .ToArea(".lei")
                .Union((qb) => 
                    qb.WayByTag("area.lei")
                    .WithTag("waterway", "canal")
                    .RecurseDown()
                    ) 
                .Output()
                .BuildQuery();

        var expected = $"[out:json];" +
            $"relation(8485220);" +
            $"map_to_area -> .lei;" +
            $"(" +
                $"way[\"waterway\"=\"canal\"](area.lei);" +
                $">;" +
            $");" +
            $"out body;";

        Assert.Equal(expected, query);
    }
}
