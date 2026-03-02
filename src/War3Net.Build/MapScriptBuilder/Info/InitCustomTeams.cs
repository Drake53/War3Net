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
        protected internal virtual void GenerateInitCustomTeams(Map map, IndentedTextWriter writer)
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
                throw new ArgumentException($"Function '{GeneratedFunctionName.InitCustomTeams}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.InitCustomTeams);

            var forceDataCount = mapInfo.Forces.Count;
            var useBlizzardAllianceFunctions = mapInfo.FormatVersion > MapInfoFormatVersion.v15;

            for (var i = 0; i < forceDataCount; i++)
            {
                var forceData = mapInfo.Forces[i];

                var playerSlots = mapInfo.Players
                    .Where(player => forceData.Players[player.Id])
                    .Select(player => player.Id)
                    .ToList();

                if (playerSlots.Count == 0)
                {
                    continue;
                }

                writer.WriteComment($"Force: {forceData.Name}");

                var alliedVictory = forceData.Flags.HasFlag(ForceFlags.AlliedVictory);
                foreach (var playerSlot in playerSlots)
                {
                    var playerExpr = JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerSlot));

                    writer.WriteCall(
                        NativeName.SetPlayerTeam,
                        playerExpr,
                        JassLiteral.Int(i));

                    if (alliedVictory)
                    {
                        writer.WriteCall(
                            NativeName.SetPlayerState,
                            playerExpr,
                            PlayerStateName.AlliedVictory,
                            "1");
                    }
                }

                var playerSlotPairs = playerSlots.SelectMany(slot1 => playerSlots.Where(slot2 => slot1 != slot2).Select(slot2 => (slot1, slot2))).ToArray();

                if (useBlizzardAllianceFunctions)
                {
                    void WriteSetAllianceStateStatement(string statementName)
                    {
                        foreach (var (playerSlot1, playerSlot2) in playerSlotPairs)
                        {
                            writer.WriteCall(
                                statementName,
                                JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerSlot1)),
                                JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerSlot2)),
                                JassKeyword.True);
                        }
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.Allied))
                    {
                        if (mapInfo.FormatVersion >= MapInfoFormatVersion.v31)
                        {
                            writer.WriteLine();
                            writer.WriteComment("  Allied");
                        }

                        WriteSetAllianceStateStatement(FunctionName.SetPlayerAllianceStateAllyBJ);
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareVision))
                    {
                        WriteSetAllianceStateStatement(FunctionName.SetPlayerAllianceStateVisionBJ);
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareUnitControl))
                    {
                        WriteSetAllianceStateStatement(FunctionName.SetPlayerAllianceStateControlBJ);
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareAdvancedUnitControl))
                    {
                        WriteSetAllianceStateStatement(FunctionName.SetPlayerAllianceStateFullControlBJ);
                    }
                }
                else
                {
                    void WriteSetAllianceStateStatement(string variableName, string comment)
                    {
                        writer.WriteLine();
                        writer.WriteComment(comment);

                        foreach (var (playerSlot1, playerSlot2) in playerSlotPairs)
                        {
                            writer.WriteCall(
                                NativeName.SetPlayerAlliance,
                                JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerSlot1)),
                                JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerSlot2)),
                                variableName,
                                JassKeyword.True);
                        }
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.Allied))
                    {
                        WriteSetAllianceStateStatement(AllianceTypeName.Passive, "  Allied");
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareVision))
                    {
                        WriteSetAllianceStateStatement(AllianceTypeName.SharedVision, "  Shared Vision");
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareUnitControl))
                    {
                        WriteSetAllianceStateStatement(AllianceTypeName.SharedControl, "  Shared Control");
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareAdvancedUnitControl))
                    {
                        WriteSetAllianceStateStatement(AllianceTypeName.SharedAdvancedControl, "  Advanced Control");
                    }
                }

                writer.WriteLine();
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateInitCustomTeams(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null;
        }

        protected internal virtual bool ShouldCallInitCustomTeams(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.MapFlags.HasFlag(MapFlags.UseCustomForces);
        }
    }
}