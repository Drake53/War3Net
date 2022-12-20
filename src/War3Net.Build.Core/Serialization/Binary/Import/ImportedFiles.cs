// ------------------------------------------------------------------------------
// <copyright file="ImportedFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Import
{
    public abstract partial class ImportedFiles
    {
        internal ImportedFiles(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<ImportedFilesFormatVersion>();

            nint importedFileCount = reader.ReadInt32();
            for (nint i = 0; i < importedFileCount; i++)
            {
                Files.Add(reader.ReadImportedFile(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(Files.Count);
            foreach (var file in Files)
            {
                writer.Write(file, FormatVersion);
            }
        }
    }
}