// ------------------------------------------------------------------------------
// <copyright file="CampaignImportedFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Import
{
    public sealed partial class CampaignImportedFiles : ImportedFiles
    {
        public const string FileName = "war3campaign.imp";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignImportedFiles"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignImportedFiles(ImportedFilesFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}