// ------------------------------------------------------------------------------
// <copyright file="PathingMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public class PathingMap
    {
        public const string FileName = "war3map.wpm"; // can also be a TGA image, war3mapPath.tga, where red=walk, green=fly, blue=build (0=yes, 255=no, alpha always 0)

        public static readonly int FileFormatSignature = "MP3W".FromRawcode();

        /// <summary>
        /// Initializes a new instance of the <see cref="PathingMap"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public PathingMap(MapPathingMapFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal PathingMap(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public MapPathingMapFormatVersion FormatVersion { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public List<PathingType> Cells { get; init; } = new();

        internal void ReadFrom(BinaryReader reader)
        {
            if (reader.ReadInt32() != FileFormatSignature)
            {
                throw new InvalidDataException($"Expected file header signature at the start of a '{FileName}' file.");
            }

            FormatVersion = reader.ReadInt32<MapPathingMapFormatVersion>();

            // Width and height should be four times the size in .w3e file (since MapTile is 128x128, and cells in PathingMap are 32x32).
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Cells.Add((PathingType)reader.ReadByte());
                }
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(FileFormatSignature);
            writer.Write((int)FormatVersion);
            writer.Write(Width);
            writer.Write(Height);

            writer.Write(Cells.Cast<byte>().ToArray());
        }
    }
}