namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapDoodadsConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapDoodads);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapDoodads>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapDoodads? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapDoodads();
            }

            public override void Write(Utf8JsonWriter writer, MapDoodads value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}