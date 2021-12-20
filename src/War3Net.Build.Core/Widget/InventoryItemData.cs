// ------------------------------------------------------------------------------
// <copyright file="InventoryItemData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed class InventoryItemData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemData"/> class.
        /// </summary>
        public InventoryItemData()
        {
        }

        internal InventoryItemData(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, useNewFormat);
        }

        // 0-indexed
        public int Slot { get; set; }

        // 0x00000000 == none
        public int ItemId { get; set; }

        public override string ToString() => ItemId.ToRawcode();

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            Slot = reader.ReadInt32();
            ItemId = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.Write(Slot);
            writer.Write(ItemId);
        }
    }
}