// ------------------------------------------------------------------------------
// <copyright file="ImportedFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Import
{
    public sealed partial class ImportedFiles
    {
        public const string FileExtension = ".imp";
        public const string CampaignFileName = "war3campaign.imp";
        public const string MapFileName = "war3map.imp";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportedFiles"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public ImportedFiles(ImportedFilesFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ImportedFilesFormatVersion FormatVersion { get; set; }

        public List<ImportedFile> Files { get; init; } = new();

        public override string ToString() => $"{FileExtension} file";
    }
}