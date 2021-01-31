// ------------------------------------------------------------------------------
// <copyright file="InitCustomTeams.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Api.Common;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public static partial class MapScriptFactory
    {
        public static JassFunctionDeclarationSyntax InitCustomTeams(MapInfo mapInfo)
        {
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

                statements.Add(new JassCommentStatementSyntax($" Force: {forceData.Name}"));

                var alliedVictory = forceData.Flags.HasFlag(ForceFlags.AlliedVictory);
                foreach (var playerSlot in playerSlots)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(SetPlayerTeam),
                        SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerSlot)),
                        SyntaxFactory.LiteralExpression(i)));

                    if (alliedVictory)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            nameof(SetPlayerState),
                            SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerSlot)),
                            SyntaxFactory.VariableReferenceExpression(nameof(PLAYER_STATE_ALLIED_VICTORY)),
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
                                SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerSlot1)),
                                SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerSlot2)),
                                SyntaxFactory.LiteralExpression(true)));
                        }
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.Allied))
                    {
                        AddSetAllianceStateStatement(nameof(War3Api.Blizzard.SetPlayerAllianceStateAllyBJ));
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareVision))
                    {
                        AddSetAllianceStateStatement(nameof(War3Api.Blizzard.SetPlayerAllianceStateVisionBJ));
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareUnitControl))
                    {
                        AddSetAllianceStateStatement(nameof(War3Api.Blizzard.SetPlayerAllianceStateControlBJ));
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareAdvancedUnitControl))
                    {
                        AddSetAllianceStateStatement(nameof(War3Api.Blizzard.SetPlayerAllianceStateFullControlBJ));
                    }
                }
                else
                {
                    void AddSetAllianceStateStatement(string variableName, string comment)
                    {
                        statements.Add(JassEmptyStatementSyntax.Value);
                        statements.Add(new JassCommentStatementSyntax(comment));

                        foreach (var (playerSlot1, playerSlot2) in playerSlotPairs)
                        {
                            statements.Add(SyntaxFactory.CallStatement(
                                nameof(SetPlayerAlliance),
                                SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerSlot1)),
                                SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerSlot2)),
                                SyntaxFactory.VariableReferenceExpression(variableName),
                                SyntaxFactory.LiteralExpression(true)));
                        }
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.Allied))
                    {
                        AddSetAllianceStateStatement(nameof(ALLIANCE_PASSIVE), "   Allied");
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareVision))
                    {
                        AddSetAllianceStateStatement(nameof(ALLIANCE_SHARED_VISION), "   Shared Vision");
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareUnitControl))
                    {
                        AddSetAllianceStateStatement(nameof(ALLIANCE_SHARED_CONTROL), "   Shared Control");
                    }

                    if (forceData.Flags.HasFlag(ForceFlags.ShareAdvancedUnitControl))
                    {
                        AddSetAllianceStateStatement(nameof(ALLIANCE_SHARED_ADVANCED_CONTROL), "   Advanced Control");
                    }
                }

                statements.Add(JassEmptyStatementSyntax.Value);
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitCustomTeams)), statements);
        }
    }
}