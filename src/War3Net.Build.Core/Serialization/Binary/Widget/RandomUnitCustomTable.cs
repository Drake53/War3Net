// ------------------------------------------------------------------------------
// <copyright file="RandomUnitCustomTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class RandomUnitCustomTable : RandomUnitData
    {
        internal RandomUnitCustomTable(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, useNewFormat);
        }

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            nint randomUnitCount = reader.ReadInt32();
            for (nint i = 0; i < randomUnitCount; i++)
            {
                RandomUnits.Add(reader.ReadRandomUnitTableUnit(formatVersion, subVersion, useNewFormat));
            }
        }

        internal override void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.Write(RandomUnits.Count);
            foreach (var randomUnit in RandomUnits)
            {
                writer.Write(randomUnit, formatVersion, subVersion, useNewFormat);
            }
        }
    }
}