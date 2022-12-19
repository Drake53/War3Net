// ------------------------------------------------------------------------------
// <copyright file="InitCustomTeams.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax InitCustomTeams(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{nameof(InitCustomTeams)}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            var statements = new List<IStatementSyntax>();

            var forceDataCount = mapInfo.Forces.Count;
            var useBlizzardAllianceFunctions = mapInfo.FormatVersion > MapInfoFormatVersion.v15;

            for (var i = 0; i < forceDataCount; i++)
            {
                var forceData = mapInfo.Forces[i];

                var playerSlots = mapInfo.Players
                    .Where(player => forceData.Players[player.Id])
                    .Select(player => player.Id)
                    .ToList();

                if (!playerSlots.Any())
                {
                    continue;
                }

                statements.Add(new JassCommentSyntax($" Force: {forceData.Name}"));

                var alliedVictory = forceData.Flags.HasFlag(ForceFlags.AlliedVictory);
                foreach (var playerSlot in playerSlots)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SetPlayerTeam,
                        SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(playerSlot)),
                        SyntaxFactory.LiteralExpression(i)));

                    if (alliedVictory)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetPlayerState,
                            SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(playerSlot)),
                            SyntaxFactory.VariableReferenceExpression(PlayerStateName.AlliedVictory),
                            SyntaxFactory.LiteralExpression(1)));
                    }
                }

                var playerSlotPairs = playerSlots.SelectMany(slot1 => playerSlots.Where(slot2 => slot1 != slot2).Select(slot2 => (slot1, slot2))).ToArray();

                if (useBlizzardAllianceFunctions)
                {
                    void AddSetAllianceStateStatement(string statementName)
                    {
                        foreach (var (playerSlot1, playerSlot2) in playerSlotPairs)
                        {
                            statements.Add(SyntaxFactory.CallStatement(
                                statementName,
                                SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(playerSlot1)),
                                SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(playerSlot2)),
                                SyntaxFactory.LiteralExpression(true)));
                        }
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.Allied))
                    {
                        if (mapInfo.FormatVersion >= MapInfoFormatVersion.v31)
                        {
                            statements.Add(JassEmptySyntax.Value);
                            statements.Add(new JassCommentSyntax("   Allied"));
                        }

                        AddSetAllianceStateStatement(FunctionName.SetPlayerAllianceStateAllyBJ);
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareVision))
                    {
                        AddSetAllianceStateStatement(FunctionName.SetPlayerAllianceStateVisionBJ);
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareUnitControl))
                    {
                        AddSetAllianceStateStatement(FunctionName.SetPlayerAllianceStateControlBJ);
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareAdvancedUnitControl))
                    {
                        AddSetAllianceStateStatement(FunctionName.SetPlayerAllianceStateFullControlBJ);
                    }
                }
                else
                {
                    void AddSetAllianceStateStatement(string variableName, string comment)
                    {
                        statements.Add(JassEmptySyntax.Value);
                        statements.Add(new JassCommentSyntax(comment));

                        foreach (var (playerSlot1, playerSlot2) in playerSlotPairs)
                        {
                            statements.Add(SyntaxFactory.CallStatement(
                                NativeName.SetPlayerAlliance,
                                SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(playerSlot1)),
                                SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(playerSlot2)),
                                SyntaxFactory.VariableReferenceExpression(variableName),
                                SyntaxFactory.LiteralExpression(true)));
                        }
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.Allied))
                    {
                        AddSetAllianceStateStatement(AllianceTypeName.Passive, "   Allied");
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareVision))
                    {
                        AddSetAllianceStateStatement(AllianceTypeName.SharedVision, "   Shared Vision");
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareUnitControl))
                    {
                        AddSetAllianceStateStatement(AllianceTypeName.SharedControl, "   Shared Control");
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareAdvancedUnitControl))
                    {
                        AddSetAllianceStateStatement(AllianceTypeName.SharedAdvancedControl, "   Advanced Control");
                    }
                }

                statements.Add(JassEmptySyntax.Value);
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitCustomTeams)), statements);
        }

        protected internal virtual bool InitCustomTeamsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null;
        }

        protected internal virtual bool InitCustomTeamsInvokeCondition(Map map)
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