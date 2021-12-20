// ------------------------------------------------------------------------------
// <copyright file="CampaignDestructableObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Object
{
    public sealed class CampaignDestructableObjectData : DestructableObjectData
    {
        public const string FileName = "war3campaign.w3b";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignDestructableObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignDestructableObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        internal CampaignDestructableObjectData(BinaryReader reader)
            : base(reader)
        {
        }

        public override string ToString() => FileName;
    }
}