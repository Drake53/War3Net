using System;
using System.Linq;
using War3Net.Build.Info;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateConfig(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.Config}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.Config);

            var playerDataCount = mapInfo.Players.Count;

            writer.WriteCall(NativeName.SetMapName, JassLiteral.String(mapInfo.MapName));
            writer.WriteCall(NativeName.SetMapDescription, JassLiteral.String(mapInfo.MapDescription));
            writer.WriteCall(NativeName.SetPlayers, JassLiteral.Int(playerDataCount));
            writer.WriteCall(NativeName.SetTeams, JassLiteral.Int(playerDataCount));
            var placement = mapInfo.Players.Any(player => player.AllyHighPriorityFlags != 0 || player.AllyLowPriorityFlags != 0)
                ? PlacementName.TeamsTogether
                : PlacementName.UseMapSettings;

            writer.WriteCall(NativeName.SetGamePlacement, placement);

            writer.WriteLine();

            if (!string.IsNullOrEmpty(LobbyMusic))
            {
                writer.WriteCall(
                    NativeName.PlayMusic,
                    JassLiteral.String(LobbyMusic));
            }

            for (var i = 0; i < playerDataCount; i++)
            {
                var location = mapInfo.Players[i].StartPosition;
                writer.WriteCall(
                    NativeName.DefineStartLocation,
                    JassLiteral.Int(i),
                    JassLiteral.Real(location.X),
                    JassLiteral.Real(location.Y));
            }

            writer.WriteLine();
            writer.WriteComment("Player setup");

            if (ShouldGenerateInitCustomPlayerSlots(map))
            {
                writer.WriteCall(GeneratedFunctionName.InitCustomPlayerSlots);
            }

            if (!mapInfo.MapFlags.HasFlag(MapFlags.UseCustomForces))
            {
                if (mapInfo.FormatVersion < MapInfoFormatVersion.v15)
                {
                    var condition = JassExpression.Equal(
                        JassExpression.InvokeSpaced(NativeName.GetGameTypeSelected),
                        GameType.UseMapSettings);

                    writer.WriteIf(JassExpression.ParenthesizedCompact(condition));
                    writer.WriteCall(GeneratedFunctionName.InitCustomTeams);
                    writer.WriteElse();
                }

                for (var i = 0; i < playerDataCount; i++)
                {
                    writer.WriteCall(
                        FunctionName.SetPlayerSlotAvailable,
                        JassExpression.Invoke(NativeName.Player, JassLiteral.Int(mapInfo.Players[i].Id)),
                        MapControlName.User);
                }

                writer.WriteCall(FunctionName.InitGenericPlayerSlots);

                if (mapInfo.FormatVersion < MapInfoFormatVersion.v15)
                {
                    writer.WriteEndIf();
                }
            }

            if (ShouldGenerateInitCustomTeams(map) && ShouldCallInitCustomTeams(map))
            {
                writer.WriteCall(GeneratedFunctionName.InitCustomTeams);
            }

            if (ShouldGenerateInitAllyPriorities(map))
            {
                writer.WriteCall(GeneratedFunctionName.InitAllyPriorities);
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateConfig(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null;
        }
    }
}