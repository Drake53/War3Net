namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapEnvironmentConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapEnvironment);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapEnvironment>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapEnvironment? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapEnvironment();
            }

            public override void Write(Utf8JsonWriter writer, MapEnvironment value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}