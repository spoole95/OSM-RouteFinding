using System.Text.Json.Serialization;

namespace OverpassNet.Entities;

public class ElementCollection
{
    public object? Version { get; set; }

    public string? Generator { get; set; }

    [JsonPropertyName("osm3s")]
    public OSM3SInfo? OSM3S { get; set; }

    public string? Copyright { get; set; }

    public string? Attribution { get; set; }

    public string? License { get; set; }

    //TODO - make reading and traversing this much more efficient when > 500k elements
    public IReadOnlyCollection<Element>? Elements { get; set; }
}