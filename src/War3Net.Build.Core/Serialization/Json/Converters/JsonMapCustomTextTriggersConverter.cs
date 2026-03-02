namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapCustomTextTriggersConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapCustomTextTriggers);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapCustomTextTriggers>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapCustomTextTriggers? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapCustomTextTriggers();
            }

            public override void Write(Utf8JsonWriter writer, MapCustomTextTriggers value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}