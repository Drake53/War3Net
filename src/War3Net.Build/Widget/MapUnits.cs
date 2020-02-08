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
using System.Linq;
using System.Text;

namespace War3Net.Build.Widget
{
    public sealed class MapUnits : IEnumerable<MapUnitData>
    {
        public const string FileName = "war3mapUnits.doo";

        private readonly List<MapUnitData> _units;

        private MapWidgetsHeader _header;

        public MapUnits(IEnumerable<MapUnitData> units)
        {
            _units = new List<MapUnitData>(units);
            _header = MapWidgetsHeader.GetDefault((uint)_units.Count);
        }

        public MapUnits(params MapUnitData[] units)
        {
            _units = new List<MapUnitData>(units);
            _header = MapWidgetsHeader.GetDefault((uint)_units.Count);
        }

        private MapUnits()
        {
            _units = new List<MapUnitData>();
        }

        public static MapUnits Default => new MapUnits(Array.Empty<MapUnitData>());

        public static bool IsRequired => false;

        public MapUnitsFormatVersion FormatVersion
        {
            get
            {
                return this.All(unit => string.IsNullOrEmpty(unit.Skin))
                    ? _header.UseTftParser ? MapUnitsFormatVersion.Tft : MapUnitsFormatVersion.Roc
                    : MapUnitsFormatVersion.Reforged;
            }

            set
            {
                var haveSkin = value == MapUnitsFormatVersion.Reforged;
                foreach (var unit in _units)
                {
                    unit.Skin = haveSkin ? unit.TypeId : null;
                }

                if (value == MapUnitsFormatVersion.Roc)
                {
                    _header.Version = MapWidgetsVersion.RoC;
                    _header.SubVersion = MapWidgetsSubVersion.V9;
                }
                else
                {
                    _header.Version = MapWidgetsVersion.TFT;
                    _header.SubVersion = MapWidgetsSubVersion.V11;
                }
            }
        }

        public int Count => _units.Count;

        public static MapUnits Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var data = new MapUnits();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    data._header = MapWidgetsHeader.Parse(stream, true);
                    var unitParser = data._header.UseTftParser
                        ? (Func<Stream, bool, MapUnitData>)MapUnitData.ParseTft
                        : MapUnitData.Parse;

                    for (var i = 0; i < data._header.DataCount; i++)
                    {
                        data._units.Add(unitParser(stream, true));
                    }
                }

                return data;
            }
            catch (DecoderFallbackException e)
            {
                throw new InvalidDataException($"The {FileName} file contains invalid characters.", e);
            }
            catch (EndOfStreamException e)
            {
                throw new InvalidDataException($"The {FileName} file is missing data, or its data is invalid.", e);
            }
            catch
            {
                throw;
            }
        }

        public static void Serialize(MapUnits mapUnits, Stream stream, bool leaveOpen = false)
        {
            mapUnits.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write(MapWidgetsHeader.HeaderSignature);
                writer.Write((uint)_header.Version);
                writer.Write((uint)_header.SubVersion);

                writer.Write(_units.Count);
                foreach (var unit in _units)
                {
                    unit.WriteTo(writer, _header.UseTftParser);
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