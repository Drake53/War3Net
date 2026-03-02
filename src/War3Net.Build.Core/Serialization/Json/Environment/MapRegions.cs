namespace War3Net.Build.Environment
{
    [JsonConverter(typeof(JsonMapRegionsConverter))]
    public sealed partial class MapRegions
    {
        internal MapRegions(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapRegions(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapRegionsFormatVersion>(nameof(FormatVersion));

            var regionsElement = jsonElement.GetProperty(nameof(Regions));
            if (regionsElement.ValueKind == JsonValueKind.Null)
            {
                Protected = true;
            }
            else
            {
                foreach (var element in regionsElement.EnumerateArray())
                {
                    Regions.Add(element.GetRegion(FormatVersion));
                }
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

            if (Protected)
            {
                writer.WriteNull(nameof(Regions));
            }
            else
            {
                writer.WriteStartArray(nameof(Regions));
                foreach (var region in Regions)
                {
                    writer.Write(region, options, FormatVersion);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }
    }
}