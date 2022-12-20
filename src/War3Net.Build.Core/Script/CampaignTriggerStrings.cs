// ------------------------------------------------------------------------------
// <copyright file="CampaignTriggerStrings.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class CampaignTriggerStrings : TriggerStrings
    {
        public const string FileName = "war3campaign.wts";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignTriggerStrings"/> class.
        /// </summary>
        public CampaignTriggerStrings()
        {
        }

        public override string ToString() => FileName;
    }
}