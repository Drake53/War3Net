using System.Text.Json;

namespace War3Net.Build.Widget
{
    public abstract partial class RandomUnitData
    {
        internal abstract void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat);
    }
}