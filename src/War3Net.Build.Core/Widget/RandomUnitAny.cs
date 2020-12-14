// ------------------------------------------------------------------------------
// <copyright file="RandomUnitAny.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed class RandomUnitAny : RandomUnitData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomUnitAny"/> class.
        /// </summary>
        public RandomUnitAny()
        {
        }

        internal RandomUnitAny(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, useNewFormat);
        }

        /// <summary>
        /// Set to -1 for any level.
        /// </summary>
        public int Level { get; set; }

        public ItemClass Class { get; set; }

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            Level = reader.ReadInt24();
            Class = (ItemClass)reader.ReadByte();
        }

        internal override void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.WriteInt24(Level);
            writer.Write((byte)Class);
        }
    }
}