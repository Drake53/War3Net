// ------------------------------------------------------------------------------
// <copyright file="MapRegions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class MapRegions
    {
        public const string FileName = "war3map.w3r";

        private static readonly int ProtectionSignature = "FUCK".FromRawcode();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRegions"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapRegions(MapRegionsFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public MapRegionsFormatVersion FormatVersion { get; set; }

        public bool Protected { get; set; }

        public List<Region> Regions { get; init; } = new();

        public override string ToString() => FileName;
    }
}