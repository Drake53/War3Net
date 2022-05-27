// ------------------------------------------------------------------------------
// <copyright file="GameBuildsProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Common;
using War3Net.Build.Info;
using War3Net.Build.Serialization;

namespace War3Net.Build.Providers
{
    public static class GameBuildsProvider
    {
        private static readonly Lazy<List<GameBuild>> _builds = new(GetGameBuildsFromJson);

        public static List<GameBuild> GetGameBuilds() => _builds.Value;

        public static List<GameBuild> GetGameBuilds(GameExpansion expansion)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.GameExpansion == expansion)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(GameReleaseType releaseType)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.GameReleaseType == releaseType)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(DateTime releaseDate)
        {
            var date = releaseDate.Date;

            return GetGameBuilds()
                .Where(gameBuild => gameBuild.ReleaseDate == date)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(GamePatch patch)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.GamePatch == patch)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(Version version)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.Version == version)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(EditorVersion editorVersion)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.EditorVersion == editorVersion)
                .ToList();
        }

        private static List<GameBuild> GetGameBuildsFromJson()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.General)
            {
                AllowTrailingCommas = true,
            };

            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new JsonStringVersionConverter());

            return JsonSerializer.Deserialize<List<GameBuild>>(Resources.War3Resources.GameBuilds, options) ?? new();
        }
    }
}