// ------------------------------------------------------------------------------
// <copyright file="PathingMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.Build.Environment
{
    public class PathingMap
    {
        public const string FileName = "war3map.wpm"; // can also be a TGA image, war3mapPath.tga, where red=walk, green=fly, blue=build (0=yes, 255=no, alpha always 0)
        public const uint HeaderSignature = 0x5733504D; // "W3PM"
        public const uint LatestVersion = 0;

        private readonly List<PathingType> _cells;

        private uint _version;
        private uint _width;
        private uint _height;

        public PathingMap()
        {
            _cells = new List<PathingType>();
        }

        public PathingMap(PathingType[,] cells)
            : this()
        {
            _width = (uint)cells.GetLength(0);
            _height = (uint)cells.GetLength(1);

            for (var y = 0; y < _width; y++)
            {
                for (var x = 0; x < _height; x++)
                {
                    _cells.Add(cells[x, y]);
                }
            }
        }

        public uint Width => _width;

        public uint Height => _height;

        public static PathingMap Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var pathingMap = new PathingMap();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    if (reader.ReadUInt32() != HeaderSignature)
                    {
                        throw new InvalidDataException($"Expected file header signature at the start of a '{FileName}' file.");
                    }

                    pathingMap._version = reader.ReadUInt32();
                    if (pathingMap._version != LatestVersion)
                    {
                        throw new NotSupportedException($"Unknown version of '{FileName}': {pathingMap._version}");
                    }

                    // Width and height should be four times the size in .w3e file (since MapTile is 128x128, and cells in PathingMap are 32x32).
                    pathingMap._width = reader.ReadUInt32();
                    pathingMap._height = reader.ReadUInt32();

                    for (var y = 0; y < pathingMap._width; y++)
                    {
                        for (var x = 0; x < pathingMap._height; x++)
                        {
                            pathingMap._cells.Add((PathingType)reader.ReadByte());
                        }
                    }
                }

                return pathingMap;
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

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write(HeaderSignature);
                writer.Write(_version);
                writer.Write(_width);
                writer.Write(_height);

                foreach (var cell in _cells)
                {
                    writer.Write((byte)cell);
                }
            }
        }
    }
}