using System.IO;
using War3Net.Common.Extensions;

namespace War3Net.Build.Import
{
    public sealed partial class ImportedFile
    {
        internal ImportedFile(BinaryReader reader, ImportedFilesFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, ImportedFilesFormatVersion formatVersion)
        {
            Flags = reader.ReadByte<ImportedFileFlags>();
            FullPath = reader.ReadChars();
        }

        internal void WriteTo(BinaryWriter writer, ImportedFilesFormatVersion formatVersion)
        {
            writer.Write((byte)Flags);
            writer.WriteString(FullPath);
        }
    }
}