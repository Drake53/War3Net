// ------------------------------------------------------------------------------
// <copyright file="RandomItemTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed class RandomItemTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomItemTable"/> class.
        /// </summary>
        public RandomItemTable()
        {
        }

        internal RandomItemTable(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public int Index { get; set; }

        public string Name { get; set; }

        public List<RandomItemSet> ItemSets { get; init; } = new();

        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Index = reader.ReadInt32();
            Name = reader.ReadChars();

            nint itemSetCount = reader.ReadInt32();
            for (nint i = 0; i < itemSetCount; i++)
            {
                ItemSets.Add(reader.ReadRandomItemSet(formatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write(Index);
            writer.WriteString(Name);

            writer.Write(ItemSets.Count);
            foreach (var itemSet in ItemSets)
            {
                writer.Write(itemSet, formatVersion);
            }
        }
    }
}