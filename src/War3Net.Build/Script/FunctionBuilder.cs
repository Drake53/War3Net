// ------------------------------------------------------------------------------
// <copyright file="FunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    internal abstract class FunctionBuilder<TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private readonly FunctionBuilderData _data;

        public FunctionBuilder(FunctionBuilderData data)
        {
            _data = data;
        }

        public FunctionBuilderData Data => _data;

        public abstract TFunctionSyntax Build(
            string functionName,
            params TStatementSyntax[] statements);

        public abstract TStatementSyntax GenerateLocalDeclarationStatement(
            string variableName);

        public abstract TStatementSyntax GenerateInvocationStatementWithoutArguments(
            string functionName);

        public abstract TStatementSyntax GenerateInvocationStatementWithIntegerArgument(
            string functionName,
            int argument);

        public abstract TStatementSyntax GenerateInvocationStatementWithBooleanArgument(
            string functionName,
            bool argument);

        public abstract TStatementSyntax GenerateInvocationStatementWithStringArgument(
            string functionName,
            string argument);

        public abstract TStatementSyntax GenerateInvocationStatementWithFloatArgument(
            string functionName,
            float argument);

        public abstract TStatementSyntax GenerateInvocationStatementWithVariableArgument(
            string functionName,
            string variableName);

        public abstract TStatementSyntax GenerateInvocationStatementWithVariableAndIntegerArgument(
            string functionName,
            string variableName,
            int argument);
    }
}