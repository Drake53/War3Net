// ------------------------------------------------------------------------------
// <copyright file="JassConfigFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Net.Build.Providers.ConfigFunctionStatementsProvider<
    War3Net.Build.Script.JassConfigFunctionBuilder,
    War3Net.CodeAnalysis.Jass.Syntax.NewStatementSyntax,
    War3Net.CodeAnalysis.Jass.Syntax.FunctionSyntax>;

namespace War3Net.Build.Script
{
    internal sealed class JassConfigFunctionBuilder : JassFunctionBuilder, IConfigFunctionBuilder<NewStatementSyntax>
    {
        public JassConfigFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public string LobbyMusic { get; set; }

        public override FunctionSyntax Build()
        {
            return Build(GetConfigFunctionName, GetStatements(this).ToArray());
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateDefineStartLocationStatement(
            string functionName,
            int startLocation,
            float x,
            float y)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.ConstantExpression(startLocation),
                    JassSyntaxFactory.ConstantExpression(x),
                    JassSyntaxFactory.ConstantExpression(y)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetPlayerStartLocationStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            int startLocation)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber)),
                    JassSyntaxFactory.ConstantExpression(startLocation)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetPlayerColorStatement(
            string functionName,
            string playerFunctionName,
            string convertColorFunctionName,
            int playerNumber)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber)),
                    JassSyntaxFactory.InvocationExpression(convertColorFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber))));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetPlayerPropertyToVariableStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            string variableName)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber)),
                    JassSyntaxFactory.VariableExpression(variableName)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetPlayerRaceSelectableStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            bool raceSelectable)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber)),
                    JassSyntaxFactory.ConstantExpression(raceSelectable)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetPlayerAllianceStatement(
            string functionName,
            string playerFunctionName,
            string allianceType,
            int playerNumber1,
            int playerNumber2,
            bool enableAlliance)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber1)),
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber2)),
                    JassSyntaxFactory.VariableExpression(allianceType),
                    JassSyntaxFactory.ConstantExpression(enableAlliance)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetPlayerTeamStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber,
            int teamNumber)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber)),
                    JassSyntaxFactory.ConstantExpression(teamNumber)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetPlayerStateStatement(
            string functionName,
            string playerFunctionName,
            string playerStateType,
            int playerNumber,
            int playerState)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber)),
                    JassSyntaxFactory.VariableExpression(playerStateType),
                    JassSyntaxFactory.ConstantExpression(playerState)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetPlayerAllianceStateStatement(
            string functionName,
            string playerFunctionName,
            int playerNumber1,
            int playerNumber2,
            bool enableState)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber1)),
                    JassSyntaxFactory.InvocationExpression(playerFunctionName, JassSyntaxFactory.ConstantExpression(playerNumber2)),
                    JassSyntaxFactory.ConstantExpression(enableState)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetStartLocPrioStatement(
            string functionName,
            int startLocation,
            int slotIndex,
            int otherStartLocation,
            string priority)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.ConstantExpression(startLocation),
                    JassSyntaxFactory.ConstantExpression(slotIndex),
                    JassSyntaxFactory.ConstantExpression(otherStartLocation),
                    JassSyntaxFactory.VariableExpression(priority)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetStartLocPrioCountStatement(
            string functionName,
            int startLocation,
            int amount)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.ConstantExpression(startLocation),
                    JassSyntaxFactory.ConstantExpression(amount)));
        }
    }
}