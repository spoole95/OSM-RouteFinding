using OverpassNet.Entities;
using OverpassNet.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OverpassNet.Converters;

internal class ElementConverter : JsonConverter<Element>
{
    public override Element? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return ReadElement(JsonDocument.ParseValue(ref reader), options) ?? null;// new Element(ElementType.Node, -1);
    }

    public override void Write(Utf8JsonWriter writer, Element value, JsonSerializerOptions options)
    {
        var writeOptions = new JsonSerializerOptions(options);

        var recursiveConverter = writeOptions.Converters.FirstOrDefault(x => x is ElementConverter);
        if (recursiveConverter != null)
            writeOptions.Converters.Remove(recursiveConverter);

        switch (value)
        {
            case Node node:
                JsonSerializer.Serialize(writer, node, writeOptions);
                break;
            case Way way:
                JsonSerializer.Serialize(writer, way, writeOptions);
                break;
            case Relation relation:
                JsonSerializer.Serialize(writer, relation, writeOptions);
                break;
            default:
                JsonSerializer.Serialize(writer, value, writeOptions);
                break;
        }
    }

    private static Element? ReadElement(JsonDocument value, JsonSerializerOptions options)
    {
        //static string? toCamelCase(string? input)
        //{
        //    if (string.IsNullOrEmpty(input))
        //        return null;

        //    return char.ToUpper(input.First()) + input.Substring(1);
        //}

        if (value.RootElement.TryGetProperty("type", out var typeElement))
        {
            //if (Enum.TryParse<ElementType>(toCamelCase(typeElement.GetString()), out var type))
            //{
            switch (typeElement.GetString())
            {
                case "node"://ElementType.Node:
                    return value.ToObject<Node>(options);
                case "way":// ElementType.Way:
                    return value.ToObject<Way>(options);
                case "relation":// ElementType.Relation:
                    return value.ToObject<Relation>(options);
                default:
                    //}
                    //else
                    //{
                    throw new JsonException("Unknown Element-type");
            }
        }
        else
        {
            throw new JsonException("The type property is necessary for all Elements");
        }

        return null;
    }
}
