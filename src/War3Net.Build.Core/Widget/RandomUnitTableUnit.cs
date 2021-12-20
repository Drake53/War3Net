// ------------------------------------------------------------------------------
// <copyright file="RandomUnitTableUnit.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed class RandomUnitTableUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomUnitTableUnit"/> class.
        /// </summary>
        public RandomUnitTableUnit()
        {
        }

        internal RandomUnitTableUnit(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, useNewFormat);
        }

        public int UnitId { get; set; }

        public int Chance { get; set; }

        public override string ToString() => $"{UnitId.ToRawcode()} ({Chance}%)";

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            UnitId = reader.ReadInt32();
            Chance = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.Write(UnitId);
            writer.Write(Chance);
        }
    }
}