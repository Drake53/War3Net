// ------------------------------------------------------------------------------
// <copyright file="MapUnits.cs" company="Drake53">
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
    public sealed class MapUnits : IEnumerable<MapUnitData>
    {
        public const string FileName = "war3mapUnits.doo";

        private readonly List<MapUnitData> _units;

        private MapWidgetsHeader _header;

        public MapUnits()
        {
            _units = new List<MapUnitData>();
        }

        public int Count => _units.Count;

        public static MapUnits Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new MapUnits();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._header = MapWidgetsHeader.Parse(stream, true);
                Func<Stream, bool, MapUnitData> unitParser = data._header.Version switch
                {
                    MapWidgetsVersion.RoC => MapUnitData.Parse,
                    MapWidgetsVersion.TFT => MapUnitData.ParseTft,
                    _ => throw new NotSupportedException(),
                };

                for (var i = 0; i < data._header.DataCount; i++)
                {
                    data._units.Add(unitParser(stream, true));
                }
            }

            return data;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write(MapWidgetsHeader.HeaderSignature);
                writer.Write((uint)MapWidgetsHeader.LatestVersion);
                writer.Write(MapWidgetsHeader.LatestSubVersion);

                writer.Write(_units.Count);
                foreach (var unit in _units)
                {
                    unit.WriteTo(writer);
                }
            }
        }

        public IEnumerator<MapUnitData> GetEnumerator()
        {
            return ((IEnumerable<MapUnitData>)_units).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MapUnitData>)_units).GetEnumerator();
        }
    }
}