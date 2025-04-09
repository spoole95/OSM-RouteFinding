using OverpassNet.Converters;
using OverpassNet.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OverpassNet;

internal static class OsmSerializer
{
    private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        IgnoreNullValues = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    static OsmSerializer()
    {
        _serializerOptions.Converters.Add(new ElementConverter());
        _serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    public static ElementCollection? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<ElementCollection>(json, _serializerOptions);
    }

    public static async Task<ElementCollection?> Deserialize(Stream jsonStream)
    {
        return await JsonSerializer.DeserializeAsync<ElementCollection>(jsonStream, _serializerOptions);
    }

    public static string? Serialize(ElementCollection? elements)
    {
        return JsonSerializer.Serialize(elements, _serializerOptions);
    }
}
