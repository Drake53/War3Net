// ------------------------------------------------------------------------------
// <copyright file="ItemObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Object
{
    public sealed partial class ItemObjectData
    {
        public const string FileExtension = ".w3t";
        public const string CampaignFileName = "war3campaign.w3t";
        public const string CampaignSkinFileName = "war3campaignSkin.w3t";
        public const string MapFileName = "war3map.w3t";
        public const string MapSkinFileName = "war3mapSkin.w3t";

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public ItemObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<SimpleObjectModification> BaseItems { get; init; } = new();

        public List<SimpleObjectModification> NewItems { get; init; } = new();

        public override string ToString() => $"{FileExtension} file";
    }
}