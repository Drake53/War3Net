// ------------------------------------------------------------------------------
// <copyright file="CampaignMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Info
{
    public sealed partial class CampaignMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignMap"/> class.
        /// </summary>
        public CampaignMap()
        {
        }

        public string Unk { get; set; }

        public string MapFilePath { get; set; }

        public override string ToString() => MapFilePath;
    }
}