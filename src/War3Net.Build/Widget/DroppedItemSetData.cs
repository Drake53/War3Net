// ------------------------------------------------------------------------------
// <copyright file="DroppedItemSetData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Build.Info;

namespace War3Net.Build.Widget
{
    public sealed class DroppedItemSetData
    {
        private RandomItemSet _itemSet;

        public RandomItemSet ItemSet => _itemSet;

        public static DroppedItemSetData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new DroppedItemSetData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._itemSet = new RandomItemSet();
                var setSize = reader.ReadInt32();
                for (var j = 0; j < setSize; j++)
                {
                    var itemId = reader.ReadChars(4);
                    var dropChance = reader.ReadInt32();
                    data._itemSet.AddItem(dropChance, itemId);
                }
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_itemSet.Size);
            foreach (var x in _itemSet)
            {
                writer.Write(x.Item2);
                writer.Write(x.Item1);
            }
        }
    }
}