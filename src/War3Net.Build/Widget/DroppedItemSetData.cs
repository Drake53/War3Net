// ------------------------------------------------------------------------------
// <copyright file="DroppedItemSetData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Build.Widget
{
    public sealed class DroppedItemSetData
    {
        private char[] _itemId;
        private int _dropChance;

        public static DroppedItemSetData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new DroppedItemSetData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                var itemCount = reader.ReadInt32();
                for (var i = 0; i < itemCount; i++)
                {
                    data._itemId = reader.ReadChars(4); // 0x00000000 == none
                    data._dropChance = reader.ReadInt32(); // in %
                }
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_itemId);
            writer.Write(_dropChance);
        }
    }
}