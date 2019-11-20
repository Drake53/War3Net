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
        public static DroppedItemSetData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new DroppedItemSetData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                var itemCount = reader.ReadInt32();
                for (var i = 0; i < itemCount; i++)
                {
                    var itemId = reader.ReadChars(4); // 0x00000000 == none
                    var dropChance = reader.ReadInt32(); // in %
                }
            }

            return data;
        }
    }
}