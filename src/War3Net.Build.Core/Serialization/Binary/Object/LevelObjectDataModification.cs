using System.IO;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class LevelObjectDataModification : ObjectDataModification
    {
        internal LevelObjectDataModification(BinaryReader reader, ObjectDataFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, ObjectDataFormatVersion formatVersion)
        {
            Id = reader.ReadInt32();
            Type = reader.ReadInt32<ObjectDataType>();
            Level = reader.ReadInt32();
            Pointer = reader.ReadInt32();
            Value = ReadValue(reader, formatVersion);
            SanityCheck = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer, ObjectDataFormatVersion formatVersion)
        {
            writer.Write(Id);
            writer.Write((int)Type);
            writer.Write(Level);
            writer.Write(Pointer);
            WriteValue(writer, formatVersion);
            writer.Write(SanityCheck);
        }
    }
}