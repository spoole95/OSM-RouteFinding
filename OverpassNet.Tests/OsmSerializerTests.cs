using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace OverpassNet.Tests;

public class OsmSerializerTests
{
    private readonly ITestOutputHelper output;

    private bool ReserializationTest(string file)
    {
        var json = File.ReadAllText(file);
        var deserialized = OsmSerializer.Deserialize(json);
        var reserialized = OsmSerializer.Serialize(deserialized);

        var diff = new JsonDiffPatch();
        var a = JToken.Parse(json);
        var b = JToken.Parse(reserialized);

        var result = diff.Diff(a, b);
        if (result != null)
        {
            output.WriteLine($"Json not equal for file {file}: {result}");
            return false;
        }

        return true;
    }

    public OsmSerializerTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void DeserializeOsmJsonTest()
    {
        Assert.True(ReserializationTest("./Fixtures/osmSampleData.json"));
    }

    [Fact]
    public void DeserializeOverpassJSONTest()
    {
        Assert.True(ReserializationTest("./Fixtures/overpassMixed.json"));
    }
}