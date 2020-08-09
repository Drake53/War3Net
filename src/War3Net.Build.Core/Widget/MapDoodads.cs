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
using System.Linq;
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
            _header = MapWidgetsHeader.GetDefault((uint)_doodads.Count);
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

        public MapWidgetsFormatVersion FormatVersion
        {
            get
            {
                return this.All(doodad => string.IsNullOrEmpty(doodad.Skin))
                    ? _header.UseTftParser ? MapWidgetsFormatVersion.Tft : MapWidgetsFormatVersion.Roc
                    : MapWidgetsFormatVersion.Reforged;
            }

            set
            {
                var haveSkin = value == MapWidgetsFormatVersion.Reforged;
                foreach (var doodad in _doodads)
                {
                    doodad.Skin = haveSkin ? doodad.TypeId : null;
                }

                if (value == MapWidgetsFormatVersion.Roc)
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

        public int Count => _doodads.Count;

        public static MapDoodads Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var data = new MapDoodads();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    data._header = MapWidgetsHeader.Parse(stream, true);
                    Func<Stream, bool, MapDoodadData> doodadParser = data._header.Version switch
                    {
                        MapWidgetsVersion.RoC => MapDoodadData.Parse,
                        MapWidgetsVersion.TFT => MapDoodadData.ParseTft,
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

        public static void Serialize(MapDoodads mapDoodads, Stream stream, bool leaveOpen = false)
        {
            mapDoodads.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write(MapWidgetsHeader.HeaderSignature);
                writer.Write((uint)_header.Version);
                writer.Write((uint)_header.SubVersion);

                writer.Write(_doodads.Count);
                foreach (var doodad in _doodads)
                {
                    doodad.WriteTo(writer, _header.UseTftParser);
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