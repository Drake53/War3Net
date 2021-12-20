// ------------------------------------------------------------------------------
// <copyright file="CampaignUpgradeObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Object
{
    public sealed class CampaignUpgradeObjectData : UpgradeObjectData
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

        internal CampaignUpgradeObjectData(BinaryReader reader)
            : base(reader)
        {
        }

        public override string ToString() => FileName;
    }
}