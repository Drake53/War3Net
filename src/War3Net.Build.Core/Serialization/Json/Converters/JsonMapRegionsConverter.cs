namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapRegionsConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapRegions);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapRegions>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapRegions? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapRegions();
            }

            public override void Write(Utf8JsonWriter writer, MapRegions value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}