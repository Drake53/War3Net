// ------------------------------------------------------------------------------
// <copyright file="UpgradesHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private static TFunctionSyntax GenerateUpgradesHelperFunction(TBuilder builder)
        {
            return builder.Build("InitUpgrades", GetUpgradesHelperFunctionStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetUpgradesHelperFunctionStatements(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;
            for (var pid = 0; pid < mapInfo.PlayerDataCount; pid++)
            {
                var playerNumber = mapInfo.GetPlayerData(pid).PlayerNumber;
                var maxLevel = new Dictionary<string, int>();
                var researched = new Dictionary<string, int>();
                for (var i = 0; i < mapInfo.UpgradeDataCount; i++)
                {
                    var upgradeData = mapInfo.GetUpgradeData(i);
                    if (upgradeData.Availability != Info.UpgradeAvailability.Available && upgradeData.AppliesToPlayer(playerNumber))
                    {
                        if (upgradeData.Availability == Info.UpgradeAvailability.Unavailable)
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
                        else if (upgradeData.Availability == Info.UpgradeAvailability.Researched)
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
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetPlayerTechMaxAllowed),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.Player),
                            builder.GenerateIntegerLiteralExpression(playerNumber)),
                        builder.GenerateFourCCExpression(tech.Key),
                        builder.GenerateIntegerLiteralExpression(tech.Value));
                }

                foreach (var tech in researched)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetPlayerTechResearched),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.Player),
                            builder.GenerateIntegerLiteralExpression(playerNumber)),
                        builder.GenerateFourCCExpression(tech.Key),
                        builder.GenerateIntegerLiteralExpression(tech.Value + 1));
                }
            }
        }
    }
}