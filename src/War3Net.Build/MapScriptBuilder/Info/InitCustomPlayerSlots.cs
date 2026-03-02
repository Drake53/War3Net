using System;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateInitCustomPlayerSlots(Map map, IndentedTextWriter writer)
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
                throw new ArgumentException($"Function '{GeneratedFunctionName.InitCustomPlayerSlots}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.InitCustomPlayerSlots);

            var playerDataCount = mapInfo.Players.Count;

            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.Players[i];

                writer.WriteLine();
                writer.WriteComment($"Player {playerData.Id}");

                var playerExpr = JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerData.Id));
                var playerColor = JassExpression.Invoke(NativeName.ConvertPlayerColor, JassLiteral.Int(playerData.Id));

                writer.WriteCall(
                    NativeName.SetPlayerStartLocation,
                    playerExpr,
                    JassLiteral.Int(i));

                if (playerData.Flags.HasFlag(PlayerFlags.FixedStartPosition))
                {
                    writer.WriteCall(
                        NativeName.ForcePlayerStartLocation,
                        playerExpr,
                        JassLiteral.Int(i));
                }

                writer.WriteCall(
                    NativeName.SetPlayerColor,
                    playerExpr,
                    playerColor);

                writer.WriteCall(
                    NativeName.SetPlayerRacePreference,
                    playerExpr,
                    playerData.Race.GetVariableName());

                writer.WriteCall(
                    NativeName.SetPlayerRaceSelectable,
                    playerExpr,
                    JassLiteral.Bool(playerData.Flags.HasFlag(PlayerFlags.RaceSelectable) || !mapInfo.MapFlags.HasFlag(MapFlags.FixedPlayerSettingsForCustomForces)));

                writer.WriteCall(
                    NativeName.SetPlayerController,
                    playerExpr,
                    playerData.Controller.GetVariableName());

                if (playerData.Controller == PlayerController.Rescuable)
                {
                    for (var j = 0; j < playerDataCount; j++)
                    {
                        var otherPlayerData = mapInfo.Players[j];
                        if (otherPlayerData.Controller == PlayerController.User)
                        {
                            writer.WriteCall(
                                NativeName.SetPlayerAlliance,
                                playerExpr,
                                JassExpression.Invoke(NativeName.Player, JassLiteral.Int(otherPlayerData.Id)),
                                AllianceTypeName.Rescuable,
                                JassKeyword.True);
                        }
                    }
                }
            }

            writer.WriteLine();

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateInitCustomPlayerSlots(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null;
        }
    }
}