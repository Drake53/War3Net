namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonItemObjectDataConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(ItemObjectData);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<ItemObjectData>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override ItemObjectData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadItemObjectData();
            }

            public override void Write(Utf8JsonWriter writer, ItemObjectData value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}