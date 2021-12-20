// ------------------------------------------------------------------------------
// <copyright file="CampaignAbilityObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Object
{
    public sealed class CampaignAbilityObjectData : AbilityObjectData
    {
        public const string FileName = "war3campaign.w3a";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignAbilityObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignAbilityObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        internal CampaignAbilityObjectData(BinaryReader reader)
            : base(reader)
        {
        }

        public override string ToString() => FileName;
    }
}