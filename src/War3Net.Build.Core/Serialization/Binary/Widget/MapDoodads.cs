﻿// ------------------------------------------------------------------------------
// <copyright file="MapDoodads.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class MapDoodads
    {
        internal MapDoodads(BinaryReader reader)
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

            nint doodadCount = reader.ReadInt32();
            if (doodadCount > 0)
            {
                Doodads.Add(reader.ReadMapDoodadData(FormatVersion, SubVersion, out var useNewFormat));
                UseNewFormat = useNewFormat;

                for (nint i = 1; i < doodadCount; i++)
                {
                    Doodads.Add(reader.ReadMapDoodadData(FormatVersion, SubVersion, out useNewFormat));
                    if (useNewFormat != UseNewFormat)
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            SpecialDoodadVersion = reader.ReadInt32<SpecialDoodadVersion>();

            nint specialDoodads = reader.ReadInt32();
            for (nint i = 0; i < specialDoodads; i++)
            {
                SpecialDoodads.Add(reader.ReadMapSpecialDoodadData(FormatVersion, SubVersion, SpecialDoodadVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(FileFormatSignature);
            writer.Write((int)FormatVersion);
            writer.Write((int)SubVersion);

            writer.Write(Doodads.Count);
            foreach (var doodad in Doodads)
            {
                writer.Write(doodad, FormatVersion, SubVersion, UseNewFormat);
            }

            writer.Write((int)SpecialDoodadVersion);

            writer.Write(SpecialDoodads.Count);
            foreach (var specialDoodad in SpecialDoodads)
            {
                writer.Write(specialDoodad, FormatVersion, SubVersion, SpecialDoodadVersion);
            }
        }
    }
}