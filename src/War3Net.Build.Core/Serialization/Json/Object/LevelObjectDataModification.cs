namespace War3Net.Build.Object
{
    public sealed partial class LevelObjectDataModification : ObjectDataModification
    {
        internal LevelObjectDataModification(JsonElement jsonElement, ObjectDataFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal LevelObjectDataModification(ref Utf8JsonReader reader, ObjectDataFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, ObjectDataFormatVersion formatVersion)
        {
            Id = jsonElement.GetInt32(nameof(Id));
            Type = jsonElement.GetInt32<ObjectDataType>(nameof(Type));
            Level = jsonElement.GetInt32(nameof(Level));
            Pointer = jsonElement.GetInt32(nameof(Pointer));
            Value = GetValue(jsonElement, nameof(Value), formatVersion);
            SanityCheck = jsonElement.GetInt32(nameof(SanityCheck));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, ObjectDataFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, ObjectDataFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Id), Id);
            writer.WriteObject(nameof(Type), Type, options);
            writer.WriteNumber(nameof(Level), Level);
            writer.WriteNumber(nameof(Pointer), Pointer);
            WriteValue(writer, nameof(Value), formatVersion);
            writer.WriteNumber(nameof(SanityCheck), SanityCheck);

            writer.WriteEndObject();
        }
    }
}