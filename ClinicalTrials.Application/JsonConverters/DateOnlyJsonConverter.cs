using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClinicalTrials.Application.JsonConverters;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.TryParseExact(
            reader.GetString(),
            Format,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var result)
            ? result
            : default;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }
}
