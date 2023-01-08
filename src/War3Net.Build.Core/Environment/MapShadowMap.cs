// ------------------------------------------------------------------------------
// <copyright file="MapShadowMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Environment
{
    public sealed partial class MapShadowMap
    {
        public const string FileExtension = ".shd";
        public const string FileName = "war3map.shd";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapShadowMap"/> class.
        /// </summary>
        public MapShadowMap()
        {
        }

        // True = 0xff, false = 0x00
        public List<byte> Cells { get; init; } = new();

        public override string ToString() => FileName;
    }
}