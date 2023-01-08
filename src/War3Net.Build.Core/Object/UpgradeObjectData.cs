// ------------------------------------------------------------------------------
// <copyright file="UpgradeObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Object
{
    public sealed partial class UpgradeObjectData
    {
        public const string FileExtension = ".w3q";
        public const string CampaignFileName = "war3campaign.w3q";
        public const string CampaignSkinFileName = "war3campaignSkin.w3q";
        public const string MapFileName = "war3map.w3q";
        public const string MapSkinFileName = "war3mapSkin.w3q";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public UpgradeObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<LevelObjectModification> BaseUpgrades { get; init; } = new();

        public List<LevelObjectModification> NewUpgrades { get; init; } = new();

        public override string ToString() => $"{FileExtension} file";
    }
}