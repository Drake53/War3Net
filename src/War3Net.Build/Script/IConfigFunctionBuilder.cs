// ------------------------------------------------------------------------------
// <copyright file="IConfigFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    internal interface IConfigFunctionBuilder<TStatementSyntax>
    {
        string LobbyMusic { get; set; }

        TStatementSyntax GenerateDefineStartLocationStatement(
            string functionName,
            int startLocation,
            float x,
            float y);

        TStatementSyntax GenerateSetPlayerStartLocationStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            int startLocation);

        TStatementSyntax GenerateSetPlayerColorStatement(
            string functionName,
            string playerFunctionName,
            string convertColorFunctionName,
            int playerNumber);

        TStatementSyntax GenerateSetPlayerPropertyToVariableStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            string variableName);

        TStatementSyntax GenerateSetPlayerRaceSelectableStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            bool raceSelectable);

        TStatementSyntax GenerateSetPlayerAllianceStatement(
            string functionName,
            string playerFunctionName,
            string allianceType,
            int playerNumber1,
            int playerNumber2,
            bool enableAlliance);

        TStatementSyntax GenerateSetPlayerTeamStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            int teamNumber);

        TStatementSyntax GenerateSetPlayerStateStatement(
            string functionName,
            string playerFunctionName,
            string playerStateType,
            int playerNumber,
            int playerState);

        TStatementSyntax GenerateSetPlayerAllianceStateStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber1,
            int playerNumber2,
            bool enableState);

        TStatementSyntax GenerateSetStartLocPrioStatement(
            string functionName,
            int startLocation,
            int slotIndex,
            int otherStartLocation,
            string priority);

        TStatementSyntax GenerateSetStartLocPrioCountStatement(
            string functionName,
            int startLocation,
            int amount);
    }
}