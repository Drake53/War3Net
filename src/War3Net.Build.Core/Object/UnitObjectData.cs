// ------------------------------------------------------------------------------
// <copyright file="UnitObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Object
{
    public sealed partial class UnitObjectData
    {
        public const string FileExtension = ".w3u";
        public const string CampaignFileName = "war3campaign.w3u";
        public const string CampaignSkinFileName = "war3campaignSkin.w3u";
        public const string MapFileName = "war3map.w3u";
        public const string MapSkinFileName = "war3mapSkin.w3u";

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public UnitObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<SimpleObjectModification> BaseUnits { get; init; } = new();

        public List<SimpleObjectModification> NewUnits { get; init; } = new();

        public override string ToString() => $"{FileExtension} file";
    }
}