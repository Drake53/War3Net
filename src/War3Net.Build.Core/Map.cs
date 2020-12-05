// ------------------------------------------------------------------------------
// <copyright file="Map.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public sealed class Map
    {
        public MapSounds? Sounds { get; set; }

        public MapCameras? Cameras { get; set; }

        public MapEnvironment Environment { get; set; }

        public MapPreviewIcons? PreviewIcons { get; set; }

        public MapRegions? Regions { get; set; }

        public PathingMap? PathingMap { get; set; }

        public ShadowMap? ShadowMap { get; set; }

        public MapInfo Info { get; set; }

        public MapAbilityObjectData? AbilityObjectData { get; set; }

        public MapBuffObjectData? BuffObjectData { get; set; }

        public MapDestructableObjectData? DestructableObjectData { get; set; }

        public MapDoodadObjectData? DoodadObjectData { get; set; }

        public MapItemObjectData? ItemObjectData { get; set; }

        public MapUnitObjectData? UnitObjectData { get; set; }

        public MapUpgradeObjectData? UpgradeObjectData { get; set; }

        public MapCustomTextTriggers? CustomTextTriggers { get; set; }

        public MapTriggers? Triggers { get; set; }

        public TriggerStrings? TriggerStrings { get; set; }

        public MapDoodads? Doodads { get; set; }

        public MapUnits? Units { get; set; }
    }
}