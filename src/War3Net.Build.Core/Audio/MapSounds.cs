// ------------------------------------------------------------------------------
// <copyright file="MapSounds.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Audio
{
    public sealed partial class MapSounds
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

        public MapSoundsFormatVersion FormatVersion { get; set; }

        public List<Sound> Sounds { get; init; } = new();

        public override string ToString() => FileName;
    }
}