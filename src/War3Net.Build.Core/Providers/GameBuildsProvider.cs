// ------------------------------------------------------------------------------
// <copyright file="GameBuildsProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Common;
using War3Net.Build.Serialization;

namespace War3Net.Build.Providers
{
    public static class GameBuildsProvider
    {
        private static readonly Lazy<List<GameBuildVersionInfo>> _builds = new(GetGameBuildsFromJson);

        public static List<GameBuildVersionInfo> GetGameBuilds() => _builds.Value;

        private static List<GameBuildVersionInfo> GetGameBuildsFromJson()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.General)
            {
                AllowTrailingCommas = true,
            };

            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new JsonStringVersionConverter());

            return JsonSerializer.Deserialize<List<GameBuildVersionInfo>>(Resources.War3Resources.GameBuilds, options) ?? new();
        }
    }
}