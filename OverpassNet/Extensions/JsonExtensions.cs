using System.Buffers;
using System.Text.Json;

namespace OverpassNet.Extensions;

internal static class JsonExtensions
{
    public static T? ToObject<T>(this JsonElement element, JsonSerializerOptions? options = null)
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(bufferWriter))
            element.WriteTo(writer);

        return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
    }

    public static T? ToObject<T>(this JsonDocument document, JsonSerializerOptions? options = null)
    {
        return document != null ? document.RootElement.ToObject<T>(options) : throw new ArgumentNullException(nameof(document));
    }
}
