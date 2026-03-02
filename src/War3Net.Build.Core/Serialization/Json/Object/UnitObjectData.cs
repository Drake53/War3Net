using System.Text.Json;
using System.Text.Json.Serialization;
using War3Net.Build.Extensions;
using War3Net.Build.Serialization.Json;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    [JsonConverter(typeof(JsonUnitObjectDataConverter))]
    public sealed partial class UnitObjectData
    {
        internal UnitObjectData(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal UnitObjectData(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<ObjectDataFormatVersion>(nameof(FormatVersion));

            foreach (var element in jsonElement.EnumerateArray(nameof(BaseUnits)))
            {
                BaseUnits.Add(element.GetSimpleObjectModification(FormatVersion));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(NewUnits)))
            {
                NewUnits.Add(element.GetSimpleObjectModification(FormatVersion));
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(FormatVersion), FormatVersion, options);

            writer.WriteStartArray(nameof(BaseUnits));
            foreach (var unit in BaseUnits)
            {
                writer.Write(unit, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(NewUnits));
            foreach (var unit in NewUnits)
            {
                writer.Write(unit, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}