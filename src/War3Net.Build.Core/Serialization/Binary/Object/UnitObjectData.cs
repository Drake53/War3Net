namespace War3Net.Build.Object
{
    public sealed partial class UnitObjectData
    {
        internal UnitObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

            nint baseUnitsCount = reader.ReadInt32();
            for (nint i = 0; i < baseUnitsCount; i++)
            {
                BaseUnits.Add(reader.ReadSimpleObjectModification(FormatVersion));
            }

            nint newUnitsCount = reader.ReadInt32();
            for (nint i = 0; i < newUnitsCount; i++)
            {
                NewUnits.Add(reader.ReadSimpleObjectModification(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(BaseUnits.Count);
            foreach (var unit in BaseUnits)
            {
                writer.Write(unit, FormatVersion);
            }

            writer.Write(NewUnits.Count);
            foreach (var unit in NewUnits)
            {
                writer.Write(unit, FormatVersion);
            }
        }
    }
}