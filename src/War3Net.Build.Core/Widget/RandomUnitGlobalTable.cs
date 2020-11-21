// ------------------------------------------------------------------------------
// <copyright file="RandomUnitGlobalTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Widget
{
    public sealed class RandomUnitGlobalTable : RandomUnitData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomUnitGlobalTable"/> class.
        /// </summary>
        public RandomUnitGlobalTable()
        {
        }

        internal RandomUnitGlobalTable(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, useNewFormat);
        }

        public int TableId { get; set; }

        public int Column { get; set; }

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            TableId = reader.ReadInt32();
            Column = reader.ReadInt32();
        }

        internal override void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.Write(TableId);
            writer.Write(Column);
        }
    }
}