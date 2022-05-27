// ------------------------------------------------------------------------------
// <copyright file="GameBuild.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json.Serialization;

using War3Net.Build.Info;

namespace War3Net.Build.Common
{
    public class GameBuild
    {
        [JsonInclude]
        public GameExpansion GameExpansion { get; private set; }

        [JsonInclude]
        public GameReleaseType GameReleaseType { get; private set; }

        [JsonInclude]
        public DateTime? ReleaseDate { get; private set; }

        [JsonInclude]
        public GamePatch? GamePatch { get; private set; }

        [JsonInclude]
        public Version Version { get; private set; }

        [JsonInclude]
        public EditorVersion? EditorVersion { get; private set; }

        public override string ToString() => $"{GameExpansion} v{Version}";
    }
}