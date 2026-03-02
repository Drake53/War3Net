namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapUnitsConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapUnits);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapUnits>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapUnits? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapUnits();
            }

            public override void Write(Utf8JsonWriter writer, MapUnits value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}