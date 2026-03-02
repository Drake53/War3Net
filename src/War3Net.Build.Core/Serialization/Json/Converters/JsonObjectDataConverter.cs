namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonObjectDataConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(ObjectData);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<ObjectData>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override ObjectData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadObjectData();
            }

            public override void Write(Utf8JsonWriter writer, ObjectData value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}