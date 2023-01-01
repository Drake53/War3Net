// ------------------------------------------------------------------------------
// <copyright file="ImportedFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Import
{
    public sealed partial class ImportedFile
    {
        internal ImportedFile(JsonElement jsonElement, ImportedFilesFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal ImportedFile(ref Utf8JsonReader reader, ImportedFilesFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, ImportedFilesFormatVersion formatVersion)
        {
            Flags = jsonElement.GetByte<ImportedFileFlags>(nameof(Flags));
            FullPath = jsonElement.GetString(nameof(FullPath));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, ImportedFilesFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, ImportedFilesFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(Flags), Flags, options);
            writer.WriteString(nameof(FullPath), FullPath);

            writer.WriteEndObject();
        }
    }
}