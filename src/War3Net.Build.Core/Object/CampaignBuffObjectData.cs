// ------------------------------------------------------------------------------
// <copyright file="CampaignBuffObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class CampaignBuffObjectData : BuffObjectData
    {
        public const string FileName = "war3campaign.w3h";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignBuffObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignBuffObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}