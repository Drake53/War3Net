using System.Text.Json;
using War3Net.Build.Extensions;

namespace War3Net.Build.Common
{
    public sealed partial class Quadrilateral
    {
        internal Quadrilateral(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal Quadrilateral(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            BottomLeft = jsonElement.GetVector2(nameof(BottomLeft));
            TopRight = jsonElement.GetVector2(nameof(TopRight));
            TopLeft = jsonElement.GetVector2(nameof(TopLeft));
            BottomRight = jsonElement.GetVector2(nameof(BottomRight));
        }

        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.Write(nameof(BottomLeft), BottomLeft);
            writer.Write(nameof(TopRight), TopRight);
            writer.Write(nameof(TopLeft), TopLeft);
            writer.Write(nameof(BottomRight), BottomRight);

            writer.WriteEndObject();
        }
    }
}