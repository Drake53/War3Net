namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapCamerasConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapCameras);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapCameras>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapCameras? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapCameras();
            }

            public override void Write(Utf8JsonWriter writer, MapCameras value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}