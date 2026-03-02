using System.IO;

namespace War3Net.Build.Script
{
    public abstract partial class TriggerItem
    {
        internal abstract void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion);
    }
}