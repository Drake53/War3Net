// ------------------------------------------------------------------------------
// <copyright file="InventoryItemData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Build.Widget
{
    public sealed class InventoryItemData
    {
        private int _slot;
        private char[] _itemId;

        public int Slot => _slot;

        public string Id => new string(_itemId);

        public static InventoryItemData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new InventoryItemData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._slot = reader.ReadInt32(); // 0-indexed
                data._itemId = reader.ReadChars(4); // 0x00000000 == none
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_slot);
            writer.Write(_itemId);
        }
    }
}