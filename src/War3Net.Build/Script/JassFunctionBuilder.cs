// ------------------------------------------------------------------------------
// <copyright file="JassFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build.Script
{
    internal abstract class JassFunctionBuilder : FunctionBuilder<FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>
    {
        public JassFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public abstract FunctionSyntax Build();

        public sealed override FunctionSyntax Build(
            string functionName,
            params NewStatementSyntax[] statements)
        {
            return JassSyntaxFactory.Function(
                JassSyntaxFactory.FunctionDeclaration(functionName),
                GetLocalDeclarations(),
                statements);
        }

        protected LocalVariableDeclarationSyntax GenerateLocalDeclaration(string type, string name)
        {
            return JassSyntaxFactory.VariableDefinition(JassSyntaxFactory.ParseTypeName(type), name);
        }

        protected abstract LocalVariableListSyntax GetLocalDeclarations();

        public sealed override NewStatementSyntax GenerateLocalDeclarationStatement(string variableName)
        {
            // In JASS syntax, local declarations are not considered to be a statement.
            return null;
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithoutArguments(
            string functionName)
        {
            return JassSyntaxFactory.CallStatement(functionName);
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithIntegerArgument(
            string functionName,
            int argument)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.ConstantExpression(argument));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithBooleanArgument(
            string functionName,
            bool argument)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.ConstantExpression(argument));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithStringArgument(
            string functionName,
            string argument)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.ConstantExpression(argument));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithFloatArgument(
            string functionName,
            float argument)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.ConstantExpression(argument));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithVariableArgument(
            string functionName,
            string variableName)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.VariableExpression(variableName));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithVariableAndIntegerArgument(string functionName, string variableName, int argument)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.VariableExpression(variableName),
                JassSyntaxFactory.ConstantExpression(argument));
        }
    }
}