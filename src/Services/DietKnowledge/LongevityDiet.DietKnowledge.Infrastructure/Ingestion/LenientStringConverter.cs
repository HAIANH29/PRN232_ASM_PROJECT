using System.Text.Json;
using System.Text.Json.Serialization;

namespace LongevityDiet.DietKnowledge.Infrastructure.Ingestion;

// Gemini sometimes writes a text field as a bare number (for example "sourceChapter": 4).
// Reading those as strings keeps one such field from invalidating the whole JSON answer.
public sealed class LenientStringConverter : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Number => reader.GetDouble().ToString(System.Globalization.CultureInfo.InvariantCulture),
            JsonTokenType.True => "true",
            JsonTokenType.False => "false",
            JsonTokenType.Null => null,
            _ => SkipToNull(ref reader)
        };
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }

    private static string? SkipToNull(ref Utf8JsonReader reader)
    {
        reader.Skip();
        return null;
    }
}
