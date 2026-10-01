namespace War3Net.Build.Widget
{
    public sealed partial class DoodadLightData
    {
        internal DoodadLightData(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            LightIndex = jsonElement.GetUInt32(nameof(LightIndex));
            IsShadowCasting = jsonElement.GetUInt32(nameof(IsShadowCasting));
            Color = jsonElement.GetColor(nameof(Color));
            Intensity = jsonElement.GetSingle(nameof(Intensity));
            ShadowCastingStart = jsonElement.GetSingle(nameof(ShadowCastingStart));
            ShadowCastingEnd = jsonElement.GetSingle(nameof(ShadowCastingEnd));
            QuadraticFalloff = jsonElement.GetSingle(nameof(QuadraticFalloff));
            LinearFalloff = jsonElement.GetSingle(nameof(LinearFalloff));
            Damping = jsonElement.GetSingle(nameof(Damping));
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber(nameof(LightIndex), LightIndex);
            writer.WriteNumber(nameof(IsShadowCasting), IsShadowCasting);
            writer.Write(nameof(Color), Color);
            writer.WriteNumber(nameof(Intensity), Intensity);
            writer.WriteNumber(nameof(ShadowCastingStart), ShadowCastingStart);
            writer.WriteNumber(nameof(ShadowCastingEnd), ShadowCastingEnd);
            writer.WriteNumber(nameof(QuadraticFalloff), QuadraticFalloff);
            writer.WriteNumber(nameof(LinearFalloff), LinearFalloff);
            writer.WriteNumber(nameof(Damping), Damping);
            writer.WriteEndObject();
        }
    }
}