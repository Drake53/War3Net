namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonImportedFilesConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(ImportedFiles);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<ImportedFiles>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override ImportedFiles? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadImportedFiles();
            }

            public override void Write(Utf8JsonWriter writer, ImportedFiles value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}