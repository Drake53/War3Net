// ------------------------------------------------------------------------------
// <copyright file="JassFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build.Script
{
    internal sealed class JassFunctionBuilder : FunctionBuilder<FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>
    {
        public JassFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public override FunctionSyntax Build(
            string functionName,
            IEnumerable<(string type, string name)> locals,
            IEnumerable<NewStatementSyntax> statements)
        {
            var localDeclarations = locals?.Select(localDeclaration => GenerateLocalDeclaration(localDeclaration.type, localDeclaration.name)).ToArray()
                ?? System.Array.Empty<LocalVariableDeclarationSyntax>();
            return localDeclarations.Length > 0
                ? JassSyntaxFactory.Function(
                    JassSyntaxFactory.FunctionDeclaration(functionName),
                    JassSyntaxFactory.LocalVariableList(localDeclarations),
                    statements.ToArray())
                : JassSyntaxFactory.Function(
                    JassSyntaxFactory.FunctionDeclaration(functionName),
                    statements.ToArray());
        }

        public sealed override IEnumerable<FunctionSyntax> BuildMainFunction()
        {
            return Main.MainFunctionGenerator<JassFunctionBuilder, FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>.GetFunctions(this);
        }

        public sealed override IEnumerable<FunctionSyntax> BuildConfigFunction()
        {
            return Config.ConfigFunctionGenerator<JassFunctionBuilder, FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>.GetFunctions(this);
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

        public override NewStatementSyntax GenerateAssignmentStatement(string variableName, NewExpressionSyntax value)
        {
            return JassSyntaxFactory.SetStatement(variableName, JassSyntaxFactory.EqualsValueClause(value));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatement(string functionName, params NewExpressionSyntax[] args)
        {
            return JassSyntaxFactory.CallStatement(functionName, args);
        }

        public override NewStatementSyntax GenerateIfStatement(NewExpressionSyntax condition, params NewStatementSyntax[] ifBody)
        {
            // TODO: add JassSyntaxFactory.IfStatement method
            return new NewStatementSyntax(
                new StatementSyntax(
                    new IfStatementSyntax(
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.IfKeyword), 0),
                        condition,
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.ThenKeyword), 0),
                        new LineDelimiterSyntax(new EndOfLineSyntax(new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.NewlineSymbol), 0))),
                        new StatementListSyntax(ifBody),
                        new CodeAnalysis.Jass.EmptyNode(0),
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.EndifKeyword), 0))),
                new LineDelimiterSyntax(new EndOfLineSyntax(new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.NewlineSymbol), 0))));
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

        public override NewExpressionSyntax GenerateBinaryExpression(BinaryOperator @operator, NewExpressionSyntax left, NewExpressionSyntax right)
        {
            var operatorTokenType = @operator switch
            {
                BinaryOperator.Addition => CodeAnalysis.Jass.SyntaxTokenType.PlusOperator,
                BinaryOperator.Subtraction => CodeAnalysis.Jass.SyntaxTokenType.MinusOperator,
                BinaryOperator.Multiplication => CodeAnalysis.Jass.SyntaxTokenType.MultiplicationOperator,
                BinaryOperator.Division => CodeAnalysis.Jass.SyntaxTokenType.DivisionOperator,

                BinaryOperator.Equals => CodeAnalysis.Jass.SyntaxTokenType.EqualityOperator,
                BinaryOperator.NotEquals => CodeAnalysis.Jass.SyntaxTokenType.UnequalityOperator,

                _ => throw new System.ArgumentException($"Binary operator {@operator} is not supported, or not defined", nameof(@operator)),
            };

            // TODO: add JassSyntaxFactory.BinaryExpression method
            return new NewExpressionSyntax(
                left.Expression,
                new BinaryExpressionTailSyntax(
                    new BinaryOperatorSyntax(new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(operatorTokenType), 0)),
                    right));
        }
    }
}