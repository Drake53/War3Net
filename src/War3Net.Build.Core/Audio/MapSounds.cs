// ------------------------------------------------------------------------------
// <copyright file="MapSounds.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Audio
{
    public sealed class MapSounds
    {
        public const string FileName = "war3map.w3s";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSounds"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapSounds(MapSoundsFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal MapSounds(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public MapSoundsFormatVersion FormatVersion { get; set; }

        public List<Sound> Sounds { get; init; } = new();

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<MapSoundsFormatVersion>();

            nint soundCount = reader.ReadInt32();
            for (nint i = 0; i < soundCount; i++)
            {
                Sounds.Add(reader.ReadSound(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(Sounds.Count);
            foreach (var sound in Sounds)
            {
                writer.Write(sound, FormatVersion);
            }
        }
    }
}