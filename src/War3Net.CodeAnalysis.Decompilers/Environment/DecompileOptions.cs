// ------------------------------------------------------------------------------
// 
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// 
// ------------------------------------------------------------------------------

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Widget;

namespace War3Net.CodeAnalysis.Decompilers
{
    public class DecompileOptions
    {
        public MapCamerasFormatVersion mapCamerasFormatVersion;
        public bool mapCamerasUseNewFormat;
        public MapRegionsFormatVersion mapRegionsFormatVersion;
        public MapSoundsFormatVersion mapSoundsFormatVersion;
        public MapWidgetsFormatVersion mapWidgetsFormatVersion;
        public MapWidgetsSubVersion mapWidgetsSubVersion;
        public bool mapWidgetsUseNewFormat = default;
        public SpecialDoodadVersion specialDoodadVersion;
    }
}