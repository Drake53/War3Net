using System.IO;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class CampaignMap
    {
        internal CampaignMap(BinaryReader reader, CampaignInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, CampaignInfoFormatVersion formatVersion)
        {
            Unk = reader.ReadChars();
            MapFilePath = reader.ReadChars();
        }

        internal void WriteTo(BinaryWriter writer, CampaignInfoFormatVersion formatVersion)
        {
            writer.WriteString(Unk);
            writer.WriteString(MapFilePath);
        }
    }
}