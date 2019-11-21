// ------------------------------------------------------------------------------
// <copyright file="JassFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.Build.Providers;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build.Script
{
    internal sealed class JassFunctionBuilder : FunctionBuilder<FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>
    {
        public JassFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public sealed override FunctionSyntax Build(
            string functionName,
            params NewStatementSyntax[] statements)
        {
            return JassSyntaxFactory.Function(
                JassSyntaxFactory.FunctionDeclaration(functionName),
                JassSyntaxFactory.LocalVariableList(GenerateLocalDeclaration(nameof(War3Api.Common.unit), MainFunctionProvider.LocalUnitVariableName)), // todo: don't create local var for config func
                statements);
        }

        public sealed override FunctionSyntax BuildMainFunction()
        {
            return Build(
                MainFunctionProvider.FunctionName,
                MainFunctionStatementsProvider<JassFunctionBuilder, FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>.GetStatements(this).ToArray());
        }

        public sealed override FunctionSyntax BuildConfigFunction()
        {
            return Build(
                ConfigFunctionProvider.FunctionName,
                ConfigFunctionStatementsProvider<JassFunctionBuilder, FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>.GetStatements(this).ToArray());
        }

        protected LocalVariableDeclarationSyntax GenerateLocalDeclaration(string type, string name)
        {
            return JassSyntaxFactory.VariableDefinition(JassSyntaxFactory.ParseTypeName(type), name);
        }

        public sealed override NewStatementSyntax GenerateLocalDeclarationStatement(string variableName)
        {
            // In JASS syntax, local declarations are not considered to be a statement.
            return null;
        }

        public sealed override NewStatementSyntax GenerateInvocationStatement(string functionName, params NewExpressionSyntax[] args)
        {
            return JassSyntaxFactory.CallStatement(functionName, args);
        }

        public sealed override NewExpressionSyntax GenerateIntegerLiteralExpression(int value)
        {
            return JassSyntaxFactory.ConstantExpression(value);
        }

        public sealed override NewExpressionSyntax GenerateBooleanLiteralExpression(bool value)
        {
            return JassSyntaxFactory.ConstantExpression(value);
        }

        public sealed override NewExpressionSyntax GenerateStringLiteralExpression(string value)
        {
            return JassSyntaxFactory.ConstantExpression(value);
        }

        public sealed override NewExpressionSyntax GenerateFloatLiteralExpression(float value)
        {
            return JassSyntaxFactory.ConstantExpression(value);
        }

        public sealed override NewExpressionSyntax GenerateNullLiteralExpression()
        {
            return JassSyntaxFactory.NullExpression();
        }

        public sealed override NewExpressionSyntax GenerateVariableExpression(string variableName)
        {
            return JassSyntaxFactory.VariableExpression(variableName);
        }

        public sealed override NewExpressionSyntax GenerateInvocationExpression(string functionName, params NewExpressionSyntax[] args)
        {
            return JassSyntaxFactory.InvocationExpression(functionName, args);
        }

        public sealed override NewExpressionSyntax GenerateFourCCExpression(string fourCC)
        {
            return JassSyntaxFactory.FourCCExpression(fourCC);
        }

        public sealed override NewExpressionSyntax GenerateBinaryAdditionExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return JassSyntaxFactory.BinaryAdditionExpression(left, right);
        }

        public sealed override NewExpressionSyntax GenerateBinarySubtractionExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return JassSyntaxFactory.BinarySubtractionExpression(left, right);
        }
    }
}