// ------------------------------------------------------------------------------
// <copyright file="FunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System.Collections.Generic;

namespace War3Net.Build.Script
{
    internal abstract class FunctionBuilder<TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TExpressionSyntax : class
    {
        private readonly FunctionBuilderData _data;

        public FunctionBuilder(FunctionBuilderData data)
        {
            _data = data;
        }

        public FunctionBuilderData Data => _data;

        public abstract string GetTypeName(
            BuiltinType type);

        public abstract TGlobalDeclarationSyntax GenerateGlobalDeclaration(
            string typeName,
            string name,
            bool isArray);

        public abstract TGlobalDeclarationSyntax GenerateGlobalDeclaration(
            string typeName,
            string name,
            TExpressionSyntax value);

        public TFunctionSyntax Build(
            string functionName,
            IEnumerable<TStatementSyntax> statements)
        {
            return Build(functionName, (IEnumerable<(string type, string name)>)null, statements);
        }

        // Note: locals typenames are for JASS, but should be abstracted away (doesn't matter for now though since don't use typename when declaring local in lua)
        // Example, instead of string "integer", should create new enum similar to Unary/BinaryOperator, then: SyntaxToken.GetDefaultTokenValue(SyntaxTokenType.IntegerKeyword)
        public abstract TFunctionSyntax Build(
            string functionName,
            IEnumerable<(string type, string name)> locals,
            IEnumerable<TStatementSyntax> statements);

        public abstract TFunctionSyntax Build(
            string functionName,
            IEnumerable<(string type, string name, TExpressionSyntax? value)> locals,
            IEnumerable<TStatementSyntax> statements);

        public abstract IEnumerable<TFunctionSyntax> BuildMainFunction();

        public abstract IEnumerable<TFunctionSyntax> BuildConfigFunction();

        public abstract TStatementSyntax GenerateAssignmentStatement(
            string variableName,
            TExpressionSyntax value);

        public abstract TStatementSyntax GenerateAssignmentStatement(
            string variableName,
            TExpressionSyntax arrayIndex,
            TExpressionSyntax value);

        public abstract TStatementSyntax GenerateInvocationStatement(
            string functionName,
            params TExpressionSyntax[] args);

        public abstract TStatementSyntax GenerateIfStatement(
            TExpressionSyntax condition,
            params TStatementSyntax[] ifBody);

        public abstract TStatementSyntax GenerateElseClause(
            TStatementSyntax ifStatement,
            TExpressionSyntax? condition,
            params TStatementSyntax[] elseBody);

        public abstract TExpressionSyntax GenerateIntegerLiteralExpression(
            int value);

        public abstract TExpressionSyntax GenerateBooleanLiteralExpression(
            bool value);

        public abstract TExpressionSyntax GenerateStringLiteralExpression(
            string value);

        public abstract TExpressionSyntax GenerateFloatLiteralExpression(
            float value);

        public abstract TExpressionSyntax GenerateFloatLiteralExpression(
            float value,
            int decimalPlaces);

        public abstract TExpressionSyntax GenerateNullLiteralExpression();

        public abstract TExpressionSyntax GenerateVariableExpression(
            string variableName);

        public abstract TExpressionSyntax GenerateInvocationExpression(
            string functionName,
            params TExpressionSyntax[] args);

        public abstract TExpressionSyntax GenerateFourCCExpression(
            string fourCC);

        public abstract TExpressionSyntax GenerateFunctionReferenceExpression(
            string functionName);

        public abstract TExpressionSyntax GenerateArrayReferenceExpression(
            string variableName,
            TExpressionSyntax index);

        public abstract TExpressionSyntax GenerateUnaryExpression(
            UnaryOperator @operator,
            TExpressionSyntax expression);

        public abstract TExpressionSyntax GenerateBinaryExpression(
            BinaryOperator @operator,
            TExpressionSyntax left,
            TExpressionSyntax right);

        public TExpressionSyntax GenerateBinaryAdditionExpression(
            TExpressionSyntax left,
            TExpressionSyntax right)
        {
            return GenerateBinaryExpression(BinaryOperator.Addition, left, right);
        }

        public TExpressionSyntax GenerateBinarySubtractionExpression(
            TExpressionSyntax left,
            TExpressionSyntax right)
        {
            return GenerateBinaryExpression(BinaryOperator.Subtraction, left, right);
        }
    }
}