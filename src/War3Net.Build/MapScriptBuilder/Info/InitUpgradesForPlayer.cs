// ------------------------------------------------------------------------------
// <copyright file="InitUpgradesForPlayer.cs" company="Drake53">
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
        protected internal virtual JassFunctionDeclarationSyntax InitUpgrades_Player(Map map, int playerId)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{nameof(InitUpgrades_Player) + playerId}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            var statements = new List<JassStatementSyntax>();

            var maxLevel = new Dictionary<int, int>();
            var researched = new Dictionary<int, int>();
            for (var i = 0; i < mapInfo.UpgradeData.Count; i++)
            {
                var upgradeData = mapInfo.UpgradeData[i];
                if (upgradeData.Availability != UpgradeAvailability.Available && upgradeData.Players[playerId])
                {
                    if (upgradeData.Availability == UpgradeAvailability.Unavailable)
                    {
                        if (maxLevel.TryGetValue(upgradeData.Id, out var level))
                        {
                            if (upgradeData.Level < level)
                            {
                                maxLevel[upgradeData.Id] = upgradeData.Level;
                            }
                        }
                        else
                        {
                            maxLevel.Add(upgradeData.Id, upgradeData.Level);
                        }
                    }
                    else if (upgradeData.Availability == UpgradeAvailability.Researched)
                    {
                        if (researched.TryGetValue(upgradeData.Id, out var level))
                        {
                            if (upgradeData.Level > level)
                            {
                                researched[upgradeData.Id] = upgradeData.Level;
                            }
                        }
                        else
                        {
                            researched.Add(upgradeData.Id, upgradeData.Level);
                        }
                    }
                }
            }

            foreach (var tech in maxLevel)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetPlayerTechMaxAllowed,
                    SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerId))),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral(tech.Key)),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(tech.Value))));
            }

            foreach (var tech in researched)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetPlayerTechResearched,
                    SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerId))),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral(tech.Key)),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(tech.Value + 1))));
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitUpgrades_Player) + playerId), statements);
        }

        protected internal virtual bool InitUpgrades_PlayerCondition(Map map, int playerId)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.UpgradeData.Any(upgradeData => upgradeData.Availability != UpgradeAvailability.Available && upgradeData.Players[playerId]);
        }
    }
}