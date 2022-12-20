// ------------------------------------------------------------------------------
// <copyright file="CampaignUpgradeObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class CampaignUpgradeObjectData : UpgradeObjectData
    {
        public const string FileName = "war3campaign.w3q";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignUpgradeObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignUpgradeObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}