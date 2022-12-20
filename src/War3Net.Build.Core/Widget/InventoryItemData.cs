// ------------------------------------------------------------------------------
// <copyright file="InventoryItemData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class InventoryItemData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemData"/> class.
        /// </summary>
        public InventoryItemData()
        {
        }

        // 0-indexed
        public int Slot { get; set; }

        // 0x00000000 == none
        public int ItemId { get; set; }

        public override string ToString() => ItemId.ToRawcode();
    }
}