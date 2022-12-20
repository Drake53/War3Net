// ------------------------------------------------------------------------------
// <copyright file="MapUnits.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class MapUnits
    {
        internal MapUnits(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            if (reader.ReadInt32() != FileFormatSignature)
            {
                throw new InvalidDataException($"Expected file header signature at the start of a .doo file.");
            }

            FormatVersion = reader.ReadInt32<MapWidgetsFormatVersion>();
            SubVersion = reader.ReadInt32<MapWidgetsSubVersion>();

            nint unitCount = reader.ReadInt32();
            if (unitCount > 0)
            {
                Units.Add(reader.ReadMapUnitData(FormatVersion, SubVersion, out var useNewFormat));
                UseNewFormat = useNewFormat;

                for (nint i = 1; i < unitCount; i++)
                {
                    Units.Add(reader.ReadMapUnitData(FormatVersion, SubVersion, out useNewFormat));
                    if (useNewFormat != UseNewFormat)
                    {
                        throw new InvalidDataException();
                    }
                }
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(FileFormatSignature);
            writer.Write((int)FormatVersion);
            writer.Write((int)SubVersion);

            writer.Write(Units.Count);
            foreach (var unit in Units)
            {
                writer.Write(unit, FormatVersion, SubVersion, UseNewFormat);
            }
        }
    }
}