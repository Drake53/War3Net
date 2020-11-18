// ------------------------------------------------------------------------------
// <copyright file="MapRegions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed class MapRegions
    {
        public const string FileName = "war3map.w3r";

        private static readonly int ProtectionSignature = "FUCK".FromRawcode();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRegions"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapRegions(MapRegionsFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal MapRegions(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public MapRegionsFormatVersion FormatVersion { get; set; }

        public bool Protected { get; set; }

        public List<Region> Regions { get; init; } = new();

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<MapRegionsFormatVersion>();

            nint regionCount = reader.ReadInt32();
            if (regionCount == ProtectionSignature)
            {
                Protected = true;
            }
            else
            {
                for (nint i = 0; i < regionCount; i++)
                {
                    Regions.Add(reader.ReadRegion(FormatVersion));
                }
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            if (Protected)
            {
                writer.Write(ProtectionSignature);
            }
            else
            {
                writer.Write(Regions.Count);
                foreach (var region in Regions)
                {
                    writer.Write(region, FormatVersion);
                }
            }
        }
    }
}