// ------------------------------------------------------------------------------
// <copyright file="MapEnvironment.cs" company="Drake53">
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

using War3Net.Build.Common;
using War3Net.Build.Providers;

namespace War3Net.Build.Environment
{
    public sealed class MapEnvironment : IEnumerable<MapTile>
    {
        public const string FileName = "war3map.w3e";
        public const uint HeaderSignature = 0x21453357; // "W3E!"
        public const uint LatestVersion = 11;

        private Tileset _tileset;
        private uint _version;

        private uint _width;
        private uint _height;
        private float _left;
        private float _bottom;

        private readonly List<TerrainType> _terrainTypes;
        private readonly List<CliffType> _cliffTypes;

        private readonly List<MapTile> _tiles;

        public MapEnvironment(Tileset tileset, uint width, uint height, int cliffLevel = 2)
            : this()
        {
            if (!Enum.IsDefined(typeof(Tileset), tileset))
            {
                throw new ArgumentOutOfRangeException(nameof(tileset));
            }

            if (((width - 1) % 32) != 0 || width == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (((height - 1) % 32) != 0 || height == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            _tileset = tileset;
            _version = LatestVersion;
            _width = width;
            _height = height;
            _left = MapWidth / -2f;
            _bottom = MapHeight / -2f;

            _terrainTypes = GetDefaultTerrainTypes().ToList();
            _cliffTypes = GetDefaultCliffTypes().ToList();

            var maxx = width - 1;
            var maxy = height - 1;
            var complements = new RectangleMargins(6, 6, 4, 8);
            var edgeLeft = complements.Left;
            var edgeRight = maxx - complements.Right;
            var edgeBottom = complements.Bottom;
            var edgeTop = maxy - complements.Top;
            for (var y = 0; y < _width; y++)
            {
                for (var x = 0; x < _height; x++)
                {
                    var tile = new MapTile();

                    tile.CliffLevel = cliffLevel;
                    tile.CliffTexture = 15;
                    tile.CliffVariation = 0;

                    tile.Height = 0;
                    tile.IsBlighted = false;
                    tile.IsBoundary = false;
                    tile.IsEdgeTile = x != maxx && y != maxy && (x < edgeLeft || x >= edgeRight || y < edgeBottom || y >= edgeTop);
                    tile.IsRamp = false;
                    tile.IsWater = false;
                    tile.Texture = 0;
                    tile.Variation = 0;
                    tile.WaterHeight = 0;

                    _tiles.Add(tile);
                }
            }
        }

        private MapEnvironment()
        {
            _terrainTypes = new List<TerrainType>();
            _cliffTypes = new List<CliffType>();

            _tiles = new List<MapTile>();
        }

        public float Left
        {
            get => _left;
            set => _left = value;
        }

        public float Right
        {
            get => _left + MapWidth;
            set => _left = value - MapWidth;
        }

        public float Top
        {
            get => _bottom + MapHeight;
            set => _bottom = value - MapHeight;
        }

        public float Bottom
        {
            get => _bottom;
            set => _bottom = value;
        }

        public float MapWidth => MapTile.TileWidth * (_width - 1);

        public float MapHeight => MapTile.TileHeight * (_height - 1);

        public static MapEnvironment Default => new MapEnvironment(Tileset.LordaeronSummer, 65, 65);

        public static MapEnvironment Parse(Stream stream, bool leaveOpen = false)
        {
            var environment = new MapEnvironment();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                if (reader.ReadUInt32() != HeaderSignature)
                {
                    throw new Exception();
                }

                environment._version = reader.ReadUInt32();

                if (environment._version != LatestVersion)
                {
                    throw new Exception();
                }

                environment._tileset = (Tileset)reader.ReadChar();
                /*var customTileset =*/ reader.ReadUInt32();

                var terrainTypeCount = reader.ReadUInt32();
                for (var i = 0; i < terrainTypeCount; i++)
                {
                    environment._terrainTypes.Add((TerrainType)reader.ReadUInt32());
                }

                var cliffTypeCount = reader.ReadUInt32();
                for (var i = 0; i < cliffTypeCount; i++)
                {
                    environment._cliffTypes.Add((CliffType)reader.ReadUInt32());
                }

                environment._width = reader.ReadUInt32();
                environment._height = reader.ReadUInt32();
                environment._left = reader.ReadSingle();
                environment._bottom = reader.ReadSingle();

                for (var y = 0; y < environment._width; y++)
                {
                    for (var x = 0; x < environment._height; x++)
                    {
                        environment._tiles.Add(MapTile.Parse(stream, true));
                    }
                }
            }

            return environment;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write(HeaderSignature);
                writer.Write(_version);
                writer.Write((char)_tileset);
                writer.Write(IsDefaultTileset() ? 0 : 1);

                writer.Write(_terrainTypes.Count);
                foreach (var terrainType in _terrainTypes)
                {
                    writer.Write((uint)terrainType);
                }

                writer.Write(_cliffTypes.Count);
                foreach (var cliffType in _cliffTypes)
                {
                    writer.Write((uint)cliffType);
                }

                writer.Write(_width);
                writer.Write(_height);
                writer.Write(_left);
                writer.Write(_bottom);

                foreach (var tile in _tiles)
                {
                    tile.WriteTo(writer);
                }
            }
        }

        public void ReplaceTiles(TerrainType oldType, TerrainType newType, bool swapBoth = false)
        {
            if (oldType == newType)
            {
                return;
            }

            var oldIndex = _terrainTypes.IndexOf(oldType);
            if (oldIndex == -1)
            {
                throw new ArgumentException($"{oldType.ToString()} is not part of the tileset.");
            }

            var newIndex = _terrainTypes.IndexOf(newType);
            if (newIndex == -1)
            {
                throw new ArgumentException($"{newType.ToString()} is not part of the tileset.");
            }

            foreach (var tile in _tiles)
            {
                if (tile.Texture == oldIndex)
                {
                    tile.Texture = newIndex;
                }
                else if (swapBoth && tile.Texture == newIndex)
                {
                    tile.Texture = oldIndex;
                }
            }
        }

        public bool IsDefaultTileset()
        {
            return AreListsEqual(_terrainTypes, GetDefaultTerrainTypes().ToArray())
                && AreListsEqual(_cliffTypes, GetDefaultCliffTypes().ToArray());
        }

        private static bool AreListsEqual(IList list1, IList list2)
        {
            var count = list1.Count;
            if (count != list2.Count)
            {
                return false;
            }

            for (var i = 0; i < count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<TerrainType> GetDefaultTerrainTypes()
        {
            return TerrainTypeProvider.GetTerrainTypes(_tileset);
        }

        private IEnumerable<CliffType> GetDefaultCliffTypes()
        {
            return TerrainTypeProvider.GetCliffTypes(_tileset);
        }

        public IEnumerator<MapTile> GetEnumerator()
        {
            return ((IEnumerable<MapTile>)_tiles).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MapTile>)_tiles).GetEnumerator();
        }
    }
}