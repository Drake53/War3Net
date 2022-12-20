// ------------------------------------------------------------------------------
// <copyright file="ImportedFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Import
{
    public abstract partial class ImportedFiles
    {
        internal ImportedFiles(ImportedFilesFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ImportedFilesFormatVersion FormatVersion { get; set; }

        public List<ImportedFile> Files { get; init; } = new();
    }
}