using System.Text.Json;

namespace War3Net.Build.Script
{
    public abstract partial class TriggerItem
    {
        internal abstract void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion);
    }
}