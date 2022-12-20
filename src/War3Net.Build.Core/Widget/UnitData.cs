// ------------------------------------------------------------------------------
// <copyright file="UnitData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Widget
{
    public sealed partial class UnitData : WidgetData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitData"/> class.
        /// </summary>
        public UnitData()
        {
        }

        public byte Flags { get; set; }

        public int OwnerId { get; set; }

        public byte Unk1 { get; set; }

        public byte Unk2 { get; set; }

        /// <summary>
        /// Set to -1 to use the default value.
        /// </summary>
        public int HP { get; set; } = -1;

        /// <summary>
        /// Set to -1 to use the default value.
        /// </summary>
        public int MP { get; set; } = -1;

        public int GoldAmount { get; set; }

        /// <summary>
        /// Set to -1 for default, and -2 for camp.
        /// </summary>
        public float TargetAcquisition { get; set; } = -1f;

        /// <summary>
        /// Non-hero units are level 1, items are level 0.
        /// </summary>
        public int HeroLevel { get; set; }

        /// <summary>
        /// Set to 0 to use the default value.
        /// </summary>
        public int HeroStrength { get; set; }

        /// <summary>
        /// Set to 0 to use the default value.
        /// </summary>
        public int HeroAgility { get; set; }

        /// <summary>
        /// Set to 0 to use the default value.
        /// </summary>
        public int HeroIntelligence { get; set; }

        public List<InventoryItemData> InventoryData { get; init; } = new();

        public List<ModifiedAbilityData> AbilityData { get; init; } = new();

        public RandomUnitData? RandomData { get; set; }

        public int CustomPlayerColorId { get; set; } = -1;

        /// <summary>
        /// The <see cref="Environment.Region.CreationNumber"/> of the waygate's target region.
        /// </summary>
        public int WaygateDestinationRegionId { get; set; } = -1;
    }
}