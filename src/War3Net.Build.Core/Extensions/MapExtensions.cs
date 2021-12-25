// ------------------------------------------------------------------------------
// <copyright file="MapExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Extensions
{
    public static class MapExtensions
    {
        public static void LocalizeInfo(this Map map)
        {
            var info = map.Info;
            var strings = map.TriggerStrings;
            if (info is null || strings is null)
            {
                return;
            }

            if (strings.TryGetValue(info.MapName, out var mapName))
            {
                info.MapName = mapName;
            }

            if (strings.TryGetValue(info.MapAuthor, out var mapAuthor))
            {
                info.MapAuthor = mapAuthor;
            }

            if (strings.TryGetValue(info.MapDescription, out var mapDescription))
            {
                info.MapDescription = mapDescription;
            }

            if (strings.TryGetValue(info.RecommendedPlayers, out var recommendedPlayers))
            {
                info.RecommendedPlayers = recommendedPlayers;
            }

            foreach (var player in info.Players)
            {
                if (strings.TryGetValue(player.Name, out var name))
                {
                    player.Name = name;
                }
            }

            foreach (var force in info.Forces)
            {
                if (strings.TryGetValue(force.Name, out var name))
                {
                    force.Name = name;
                }
            }
        }
    }
}