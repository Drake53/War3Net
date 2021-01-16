// ------------------------------------------------------------------------------
// <copyright file="MapUnits.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed class MapUnits
    {
        public const string FileName = "war3mapUnits.doo";

        public static readonly int FileFormatSignature = "W3do".FromRawcode();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapUnits"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="subVersion"></param>
        /// <param name="useNewFormat"></param>
        public MapUnits(MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            FormatVersion = formatVersion;
            SubVersion = subVersion;
            UseNewFormat = useNewFormat;
        }

        internal MapUnits(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public MapWidgetsFormatVersion FormatVersion { get; set; }

        public MapWidgetsSubVersion SubVersion { get; set; }

        public bool UseNewFormat { get; set; }

        public List<UnitData> Units { get; init; } = new();

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