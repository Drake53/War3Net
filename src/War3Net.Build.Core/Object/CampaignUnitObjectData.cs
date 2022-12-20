// ------------------------------------------------------------------------------
// <copyright file="CampaignUnitObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class CampaignUnitObjectData : UnitObjectData
    {
        public const string FileName = "war3campaign.w3u";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignUnitObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignUnitObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}