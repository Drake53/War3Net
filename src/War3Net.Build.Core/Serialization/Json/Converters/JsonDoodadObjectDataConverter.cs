namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonDoodadObjectDataConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DoodadObjectData);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<DoodadObjectData>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override DoodadObjectData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadDoodadObjectData();
            }

            public override void Write(Utf8JsonWriter writer, DoodadObjectData value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}