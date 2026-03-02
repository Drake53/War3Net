using System.IO;

namespace War3Net.Build.Widget
{
    public abstract partial class RandomUnitData
    {
        internal abstract void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat);
    }
}