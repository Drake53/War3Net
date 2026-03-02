namespace War3Net.Build.Environment
{
    [JsonConverter(typeof(JsonMapShadowMapConverter))]
    public sealed partial class MapShadowMap
    {
        internal MapShadowMap(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapShadowMap(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            foreach (var element in jsonElement.EnumerateArray(nameof(Cells)))
            {
                Cells.Add(element.GetByte());
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(Cells), Cells, options);

            writer.WriteEndObject();
        }
    }
}