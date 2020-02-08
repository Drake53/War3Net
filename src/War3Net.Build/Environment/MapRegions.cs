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
        public const MapRegionsFormatVersion LatestVersion = MapRegionsFormatVersion.Normal;

        private readonly List<Region> _regions;

        private MapRegionsFormatVersion _version;

        public MapRegions(params Region[] regions)
        {
            _regions = new List<Region>(regions);
            _version = LatestVersion;
        }

        private MapRegions()
        {
            _regions = new List<Region>();
        }

        public MapRegionsFormatVersion FormatVersion
        {
            get => _version;
            set => _version = value;
        }

        public static MapRegions Default => new MapRegions(Array.Empty<Region>());

        public static bool IsRequired => false;

        public int Count => _regions.Count;

        public static MapRegions Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var mapRegions = new MapRegions();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    mapRegions._version = (MapRegionsFormatVersion)reader.ReadUInt32();
                    if (mapRegions._version != LatestVersion)
                    {
                        throw new NotSupportedException($"Unknown version of '{FileName}': {mapRegions._version}");
                    }

                    var regionCount = reader.ReadUInt32();
                    for (var i = 0; i < regionCount; i++)
                    {
                        mapRegions._regions.Add(Region.Parse(stream, true));
                    }
                }

                return mapRegions;
            }
            catch (DecoderFallbackException e)
            {
                throw new InvalidDataException($"The '{FileName}' file contains invalid characters.", e);
            }
            catch (EndOfStreamException e)
            {
                throw new InvalidDataException($"The '{FileName}' file is missing data, or its data is invalid.", e);
            }
            catch
            {
                throw;
            }
        }

        public static void Serialize(MapRegions mapRegions, Stream stream, bool leaveOpen = false)
        {
            mapRegions.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write((uint)_version);

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