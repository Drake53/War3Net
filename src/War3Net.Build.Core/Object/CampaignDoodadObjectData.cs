// ------------------------------------------------------------------------------
// <copyright file="CampaignDoodadObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class CampaignDoodadObjectData : DoodadObjectData
    {
        public const string FileName = "war3campaign.w3d";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignDoodadObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignDoodadObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}