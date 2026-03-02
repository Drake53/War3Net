namespace War3Net.Build.Info
{
    public sealed partial class RandomUnitSet
    {
        internal RandomUnitSet(BinaryReader reader, MapInfoFormatVersion formatVersion, int setSize)
        {
            UnitIds = new int[setSize];
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Chance = reader.ReadInt32();
            for (nint i = 0; i < UnitIds.Length; i++)
            {
                UnitIds[i] = reader.ReadInt32();
            }
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write(Chance);
            for (nint i = 0; i < UnitIds.Length; i++)
            {
                writer.Write(UnitIds[i]);
            }
        }
    }
}