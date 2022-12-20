// ------------------------------------------------------------------------------
// <copyright file="MapPathingMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class MapPathingMap
    {
        public const string FileName = "war3map.wpm"; // can also be a TGA image, war3mapPath.tga, where red=walk, green=fly, blue=build (0=yes, 255=no, alpha always 0)

        public static readonly int FileFormatSignature = "MP3W".FromRawcode();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPathingMap"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapPathingMap(MapPathingMapFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public MapPathingMapFormatVersion FormatVersion { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public List<PathingType> Cells { get; init; } = new();

        public override string ToString() => FileName;
    }
}