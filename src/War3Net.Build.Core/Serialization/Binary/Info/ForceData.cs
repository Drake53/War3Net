namespace War3Net.Build.Info
{
    public sealed partial class ForceData
    {
        internal ForceData(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Flags = reader.ReadInt32<ForceFlags>();
            Players = reader.ReadBitmask32();
            Name = reader.ReadChars();
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write((int)Flags);
            writer.Write(Players);
            writer.WriteString(Name);
        }
    }
}