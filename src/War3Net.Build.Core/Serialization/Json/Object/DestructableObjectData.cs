namespace War3Net.Build.Object
{
    [JsonConverter(typeof(JsonDestructableObjectDataConverter))]
    public sealed partial class DestructableObjectData
    {
        internal DestructableObjectData(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal DestructableObjectData(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<ObjectDataFormatVersion>(nameof(FormatVersion));

            foreach (var element in jsonElement.EnumerateArray(nameof(BaseDestructables)))
            {
                BaseDestructables.Add(element.GetSimpleObjectModification(FormatVersion));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(NewDestructables)))
            {
                NewDestructables.Add(element.GetSimpleObjectModification(FormatVersion));
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

            writer.WriteStartArray(nameof(BaseDestructables));
            foreach (var destructable in BaseDestructables)
            {
                writer.Write(destructable, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(NewDestructables));
            foreach (var destructable in NewDestructables)
            {
                writer.Write(destructable, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}