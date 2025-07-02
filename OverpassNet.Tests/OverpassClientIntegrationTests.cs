using OverpassNet.Query;
using System.ComponentModel;
using Xunit.Abstractions;

namespace OverpassNet.Tests;

public class OverpassClientIntegrationTests
{
    private readonly ITestOutputHelper _outputHelper;
    public OverpassClientIntegrationTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact, Category("Integration")]
    public async Task CanalsInArea_ShouldQuery()
    {
        var result = await new OverpassQueryBuilder()
                .Relation(8485220)
                .ToArea(".lei")
                .Union((qb) =>
                    qb.WayByTag("area.lei")
                    .WithTag("waterway", "canal")
                    .RecurseDown()
                    )
                .Output()
                .GetAsync();

        _outputHelper.WriteLine($"{result.Elements.Count} elements returned");
        Assert.True(result.Elements?.Count > 0);
    }
}
