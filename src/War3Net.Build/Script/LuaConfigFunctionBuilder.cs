// ------------------------------------------------------------------------------
// <copyright file="LuaConfigFunctionBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using CSharpLua.LuaAst;

using static War3Net.Build.Providers.ConfigFunctionStatementsProvider<
    War3Net.Build.Script.LuaConfigFunctionBuilder,
    CSharpLua.LuaAst.LuaStatementSyntax,
    CSharpLua.LuaAst.LuaVariableListDeclarationSyntax>;

namespace War3Net.Build.Script
{
    internal sealed class LuaConfigFunctionBuilder : LuaFunctionBuilder, IConfigFunctionBuilder<LuaStatementSyntax>
    {
        public LuaConfigFunctionBuilder(MapInfo mapInfo)
            : base(mapInfo)
        {
        }

        public string LobbyMusic { get; set; }

        public override LuaVariableListDeclarationSyntax Build()
        {
            return Build(GetConfigFunctionName, GetStatements(this).ToArray());
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateDefineStartLocationStatement(
            string functionName,
            int startLocation,
            float x,
            float y)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
               functionName,
               new LuaFloatLiteralExpressionSyntax(startLocation),
               new LuaFloatLiteralExpressionSyntax(x),
               new LuaFloatLiteralExpressionSyntax(y)));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetPlayerStartLocationStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            int startLocation)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber),
                startLocation));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetPlayerColorStatement(
            string functionName,
            string playerFunctionName,
            string convertColorFunctionName,
            int playerNumber)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber),
                new LuaInvocationExpressionSyntax(convertColorFunctionName, playerNumber)));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetPlayerPropertyToVariableStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            string variableName)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber),
                new LuaIdentifierLiteralExpressionSyntax(variableName)));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetPlayerRaceSelectableStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            bool raceSelectable)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber),
                raceSelectable ? LuaIdentifierLiteralExpressionSyntax.True : LuaIdentifierLiteralExpressionSyntax.False));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetPlayerAllianceStatement(
            string functionName,
            string playerFunctionName,
            string allianceType,
            int playerNumber1,
            int playerNumber2,
            bool enableAlliance)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber1),
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber2),
                allianceType,
                enableAlliance ? LuaIdentifierLiteralExpressionSyntax.True : LuaIdentifierLiteralExpressionSyntax.False));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetPlayerTeamStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            int teamNumber)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber),
                teamNumber));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetPlayerStateStatement(
            string functionName,
            string playerFunctionName,
            string playerStateType,
            int playerNumber,
            int playerState)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber),
                playerStateType,
                playerState));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetPlayerAllianceStateStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber1,
            int playerNumber2,
            bool enableState)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber1),
                new LuaInvocationExpressionSyntax(playerFunctionName, playerNumber2),
                enableState ? LuaIdentifierLiteralExpressionSyntax.True : LuaIdentifierLiteralExpressionSyntax.False));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetStartLocPrioStatement(
            string functionName,
            int startLocation,
            int slotIndex,
            int otherStartLocation,
            string priority)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                startLocation,
                slotIndex,
                otherStartLocation,
                priority));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetStartLocPrioCountStatement(
            string functionName,
            int startLocation,
            int amount)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                startLocation,
                amount));
        }
    }
}