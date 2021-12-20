// ------------------------------------------------------------------------------
// <copyright file="CampaignItemObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Object
{
    public sealed class CampaignItemObjectData : ItemObjectData
    {
        public const string FileName = "war3campaign.w3t";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignItemObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignItemObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        internal CampaignItemObjectData(BinaryReader reader)
            : base(reader)
        {
        }

        public override string ToString() => FileName;
    }
}