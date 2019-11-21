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

        public abstract TFunctionSyntax BuildMainFunction();

        public abstract TFunctionSyntax BuildConfigFunction();

        public abstract TStatementSyntax GenerateLocalDeclarationStatement(
            string variableName);

        public abstract TStatementSyntax GenerateInvocationStatement(
            string functionName,
            params TExpressionSyntax[] args);

        public abstract TExpressionSyntax GenerateIntegerLiteralExpression(
            int value);

        public abstract TExpressionSyntax GenerateBooleanLiteralExpression(
            bool value);

        public abstract TExpressionSyntax GenerateStringLiteralExpression(
            string value);

        public abstract TExpressionSyntax GenerateFloatLiteralExpression(
            float value);

        public abstract TExpressionSyntax GenerateNullLiteralExpression();

        public abstract TExpressionSyntax GenerateVariableExpression(
            string variableName);

        public abstract TExpressionSyntax GenerateInvocationExpression(
            string functionName,
            params TExpressionSyntax[] args);

        public abstract TExpressionSyntax GenerateFourCCExpression(
            string fourCC);

        public abstract TExpressionSyntax GenerateBinaryAdditionExpression(
            TExpressionSyntax left,
            TExpressionSyntax right);

        public abstract TExpressionSyntax GenerateBinarySubtractionExpression(
            TExpressionSyntax left,
            TExpressionSyntax right);
    }
}