// ------------------------------------------------------------------------------
// <copyright file="MapCameras.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Environment
{
    public sealed partial class MapCameras
    {
        public const string FileName = "war3map.w3c";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCameras"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapCameras(MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            FormatVersion = formatVersion;
            UseNewFormat = useNewFormat;
        }

        public MapCamerasFormatVersion FormatVersion { get; set; }

        public bool UseNewFormat { get; set; }

        public List<Camera> Cameras { get; init; } = new();

        public override string ToString() => FileName;
    }
}