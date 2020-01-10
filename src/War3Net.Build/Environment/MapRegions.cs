// ------------------------------------------------------------------------------
// <copyright file="MapRegions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.Build.Environment
{
    public sealed class MapRegions : IEnumerable<Region>
    {
        public const string FileName = "war3map.w3r";
        public const uint LatestVersion = 5;

        private readonly List<Region> _regions;

        private uint _version;

        private MapRegions()
        {
            _regions = new List<Region>();
        }

        public static MapRegions Parse(Stream stream, bool leaveOpen = false)
        {
            var mapRegions = new MapRegions();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                mapRegions._version = reader.ReadUInt32();
                if (mapRegions._version != LatestVersion)
                {
                    throw new Exception();
                }

                var regionCount = reader.ReadUInt32();
                for (var i = 0; i < regionCount; i++)
                {
                    mapRegions._regions.Add(Region.Parse(stream, true));
                }
            }

            return mapRegions;
        }

        public static void Serialize(MapRegions mapRegions, Stream stream, bool leaveOpen = false)
        {
            mapRegions.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write(LatestVersion);

                writer.Write(_regions.Count);
                foreach (var region in _regions)
                {
                    region.WriteTo(writer);
                }
            }
        }

        public IEnumerator<Region> GetEnumerator()
        {
            return ((IEnumerable<Region>)_regions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Region>)_regions).GetEnumerator();
        }
    }
}