// ------------------------------------------------------------------------------
// <copyright file="Campaign.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;

namespace War3Net.Build
{
    public sealed class Campaign
    {
        public CampaignInfo Info { get; set; }

        public CampaignAbilityObjectData? AbilityObjectData { get; set; }

        public CampaignBuffObjectData? BuffObjectData { get; set; }

        public CampaignDestructableObjectData? DestructableObjectData { get; set; }

        public CampaignDoodadObjectData? DoodadObjectData { get; set; }

        public CampaignItemObjectData? ItemObjectData { get; set; }

        public CampaignUnitObjectData? UnitObjectData { get; set; }

        public CampaignUpgradeObjectData? UpgradeObjectData { get; set; }

        public CampaignTriggerStrings? TriggerStrings { get; set; }
    }
}