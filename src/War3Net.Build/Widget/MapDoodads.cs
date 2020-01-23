// ------------------------------------------------------------------------------
// <copyright file="MapDoodads.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.Build.Widget
{
    public sealed class MapDoodads : IEnumerable<MapDoodadData>
    {
        public const string FileName = "war3map.doo";

        private readonly List<MapDoodadData> _doodads;
        private readonly List<MapSpecialDoodadData> _specialDoodads;

        private MapWidgetsHeader _header;

        public MapDoodads(IEnumerable<MapDoodadData> doodads)
            : this(doodads, Array.Empty<MapSpecialDoodadData>())
        {
        }

        public MapDoodads(IEnumerable<MapDoodadData> doodads, IEnumerable<MapSpecialDoodadData> specialDoodads)
        {
            _doodads = new List<MapDoodadData>(doodads);
            _specialDoodads = new List<MapSpecialDoodadData>(specialDoodads);
        }

        public MapDoodads(params MapDoodadData[] doodads)
        {
            _doodads = new List<MapDoodadData>(doodads);
            _specialDoodads = new List<MapSpecialDoodadData>();
            _header = MapWidgetsHeader.GetDefault((uint)_doodads.Count);
        }

        private MapDoodads()
        {
            _doodads = new List<MapDoodadData>();
            _specialDoodads = new List<MapSpecialDoodadData>();
        }

        public static MapDoodads Default => new MapDoodads(Array.Empty<MapDoodadData>());

        public static bool IsRequired => false;

        public int Count => _doodads.Count;

        public static MapDoodads Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new MapDoodads();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._header = MapWidgetsHeader.Parse(stream, true);
                Func<Stream, bool, MapDoodadData> doodadParser = data._header.Version switch
                {
                    MapWidgetsVersion.RoC => MapDoodadData.Parse,
                    MapWidgetsVersion.TFT => MapDoodadData.ParseTft,
                    _ => throw new NotSupportedException(),
                };

                for (var i = 0; i < data._header.DataCount; i++)
                {
                    data._doodads.Add(doodadParser(stream, true));
                }

                var specialDoodadsVersion = reader.ReadInt32();
                if (specialDoodadsVersion != 0)
                {
                    throw new NotSupportedException($"Unknown special doodads version: {specialDoodadsVersion}.");
                }

                var specialDoodads = reader.ReadInt32();
                for (var i = 0; i < specialDoodads; i++)
                {
                    data._specialDoodads.Add(MapSpecialDoodadData.Parse(stream, true));
                }
            }

            return data;
        }

        public static void Serialize(MapDoodads mapDoodads, Stream stream, bool leaveOpen = false)
        {
            mapDoodads.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write(MapWidgetsHeader.HeaderSignature);
                writer.Write((uint)MapWidgetsHeader.LatestVersion);
                writer.Write(MapWidgetsHeader.LatestSubVersion);

                writer.Write(_doodads.Count);
                foreach (var doodad in _doodads)
                {
                    doodad.WriteTo(writer);
                }

                writer.Write(0); // specialDoodadVersion
                writer.Write(_specialDoodads.Count);
                foreach (var doodad in _specialDoodads)
                {
                    doodad.WriteTo(writer);
                }
            }
        }

        public IEnumerator<MapDoodadData> GetEnumerator()
        {
            return ((IEnumerable<MapDoodadData>)_doodads).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MapDoodadData>)_doodads).GetEnumerator();
        }
    }
}