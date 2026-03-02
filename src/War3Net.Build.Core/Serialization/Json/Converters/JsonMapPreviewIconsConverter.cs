namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapPreviewIconsConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapPreviewIcons);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapPreviewIcons>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapPreviewIcons? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapPreviewIcons();
            }

            public override void Write(Utf8JsonWriter writer, MapPreviewIcons value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}