// ------------------------------------------------------------------------------
// <copyright file="MapPreviewIcons.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed class MapPreviewIcons
    {
        public const string FileName = "war3map.mmp";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPreviewIcons"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapPreviewIcons(MapPreviewIconsFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal MapPreviewIcons(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public MapPreviewIconsFormatVersion FormatVersion { get; set; }

        public List<MapPreviewIcon> Icons { get; init; } = new();

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<MapPreviewIconsFormatVersion>();

            nint iconCount = reader.ReadInt32();
            for (nint i = 0; i < iconCount; i++)
            {
                Icons.Add(reader.ReadMapPreviewIcon(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(Icons.Count);
            foreach (var icon in Icons)
            {
                writer.Write(icon, FormatVersion);
            }
        }
    }
}