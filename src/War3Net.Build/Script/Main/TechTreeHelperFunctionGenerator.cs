// ------------------------------------------------------------------------------
// <copyright file="TechTreeHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private static TFunctionSyntax GenerateTechTreeHelperFunction(TBuilder builder)
        {
            return builder.Build("InitTechTree", GetTechTreeHelperFunctionStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetTechTreeHelperFunctionStatements(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;
            for (var pid = 0; pid < mapInfo.PlayerDataCount; pid++)
            {
                var playerNumber = mapInfo.GetPlayerData(pid).PlayerNumber;
                for (var i = 0; i < mapInfo.TechDataCount; i++)
                {
                    var techData = mapInfo.GetTechData(i);
                    if (techData.AppliesToPlayer(playerNumber))
                    {
                        if (techData.IsAbility)
                        {
                            yield return builder.GenerateInvocationStatement(
                                nameof(War3Api.Common.SetPlayerAbilityAvailable),
                                builder.GenerateInvocationExpression(
                                    nameof(War3Api.Common.Player),
                                    builder.GenerateIntegerLiteralExpression(playerNumber)),
                                builder.GenerateFourCCExpression(techData.Id),
                                builder.GenerateBooleanLiteralExpression(false));
                        }
                        else
                        {
                            yield return builder.GenerateInvocationStatement(
                                nameof(War3Api.Common.SetPlayerTechMaxAllowed),
                                builder.GenerateInvocationExpression(
                                    nameof(War3Api.Common.Player),
                                    builder.GenerateIntegerLiteralExpression(playerNumber)),
                                builder.GenerateFourCCExpression(techData.Id),
                                builder.GenerateIntegerLiteralExpression(0));
                        }
                    }
                }
            }
        }
    }
}