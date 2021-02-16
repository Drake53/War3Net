// ------------------------------------------------------------------------------
// <copyright file="ForceDataExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.Build.Common;
using War3Net.Build.Info;

namespace War3Net.Build.Extensions
{
    public static class ForceDataExtensions
    {
        public static void SetPlayers(this ForceData force, params PlayerData[] players)
        {
            force.Players = new Bitmask32(players.Select(player => player.Id).ToArray());
        }
    }
}