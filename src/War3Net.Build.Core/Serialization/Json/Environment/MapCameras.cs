namespace War3Net.Build.Environment
{
    [JsonConverter(typeof(JsonMapCamerasConverter))]
    public sealed partial class MapCameras
    {
        internal MapCameras(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapCameras(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapCamerasFormatVersion>(nameof(FormatVersion));
            UseNewFormat = jsonElement.GetBoolean(nameof(UseNewFormat));

            foreach (var element in jsonElement.EnumerateArray(nameof(Cameras)))
            {
                Cameras.Add(element.GetCamera(FormatVersion, UseNewFormat));
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
            writer.WriteBoolean(nameof(UseNewFormat), UseNewFormat);

            writer.WriteStartArray(nameof(Cameras));
            foreach (var camera in Cameras)
            {
                writer.Write(camera, options, FormatVersion, UseNewFormat);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}