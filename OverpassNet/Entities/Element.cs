using System.Text.Json.Serialization;

namespace OverpassNet.Entities;

public abstract class Element
{
    public Element(ElementType type, long id)
    {
        Type = type;
        Id = id;
    }

    public ElementType Type { get; }
    public long Id { get; }
    public IReadOnlyDictionary<string, string>? Tags { get; set; }
    public DateTime? Timestamp { get; set; }
    public int? Version { get; set; }
    public int? Changeset { get; set; }
    public string? User { get; set; }
    [JsonPropertyName("uid")]
    public ulong? UId { get; set; }

    public override string ToString()
    {
        if (Tags?.TryGetValue("name", out var nameTag) ?? false)
        {
            return nameTag;
        }
        return $"{Type} {Id}";
    }
}
