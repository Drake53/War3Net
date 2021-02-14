// ------------------------------------------------------------------------------
// <copyright file="InitUpgrades.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Api.Common;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual JassFunctionDeclarationSyntax InitUpgrades(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;

            var statements = new List<IStatementSyntax>();

            foreach (var player in mapInfo.Players)
            {
                var playerNumber = player.Id;
                var maxLevel = new Dictionary<int, int>();
                var researched = new Dictionary<int, int>();
                for (var i = 0; i < mapInfo.UpgradeData.Count; i++)
                {
                    var upgradeData = mapInfo.UpgradeData[i];
                    if (upgradeData.Availability != UpgradeAvailability.Available && upgradeData.Players[playerNumber])
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
                        nameof(SetPlayerTechMaxAllowed),
                        SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerNumber)),
                        SyntaxFactory.FourCCLiteralExpression(tech.Key),
                        SyntaxFactory.LiteralExpression(tech.Value)));
                }

                foreach (var tech in researched)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(SetPlayerTechResearched),
                        SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerNumber)),
                        SyntaxFactory.FourCCLiteralExpression(tech.Key),
                        SyntaxFactory.LiteralExpression(tech.Value + 1)));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitUpgrades)), statements);
        }

        protected virtual bool InitUpgradesCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info.UpgradeData.Any();
        }
    }
}