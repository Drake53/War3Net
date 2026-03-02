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