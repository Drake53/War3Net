namespace War3Net.Build.Info
{
    public sealed partial class RandomUnitTable
    {
        internal RandomUnitTable(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Index = reader.ReadInt32();
            Name = reader.ReadChars();

            nint typeCount = reader.ReadInt32(); // amount of columns
            for (nint x = 0; x < typeCount; x++)
            {
                Types.Add(reader.ReadInt32<WidgetType>());
            }

            nint unitSetCount = reader.ReadInt32(); // amount of rows
            for (nint y = 0; y < unitSetCount; y++)
            {
                UnitSets.Add(reader.ReadRandomUnitSet(formatVersion, (int)typeCount));
            }
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write(Index);
            writer.WriteString(Name);

            writer.Write(Types.Count);
            foreach (var type in Types)
            {
                writer.Write((int)type);
            }

            writer.Write(UnitSets.Count);
            foreach (var unitSet in UnitSets)
            {
                writer.Write(unitSet, formatVersion);
            }
        }
    }
}