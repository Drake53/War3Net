// ------------------------------------------------------------------------------
// <copyright file="JassFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build.Script
{
    internal sealed class JassFunctionBuilder : FunctionBuilder<GlobalDeclarationSyntax, FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>
    {
        public JassFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public override string GetTypeName(BuiltinType type)
        {
            return SyntaxToken.GetDefaultTokenValue(type switch
            {
                BuiltinType.Boolean => SyntaxTokenType.BooleanKeyword,
                BuiltinType.Single => SyntaxTokenType.RealKeyword,
                BuiltinType.Int32 => SyntaxTokenType.IntegerKeyword,
                BuiltinType.Object => SyntaxTokenType.HandleKeyword,
                BuiltinType.String => SyntaxTokenType.StringKeyword,

                _ => throw new NotSupportedException(),
            });
        }

        public override GlobalDeclarationSyntax? GenerateGlobalDeclaration(string typeName, string name, bool isArray)
        {
            return isArray
                ? new GlobalDeclarationSyntax(new GlobalVariableDeclarationSyntax(
                    new VariableDeclarationSyntax(
                        new ArrayDefinitionSyntax(
                            JassSyntaxFactory.ParseTypeName(typeName),
                            new TokenNode(new SyntaxToken(SyntaxTokenType.ArrayKeyword), 0),
                            new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, name), 0))),
                    new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0)))))
                : new GlobalDeclarationSyntax(new GlobalVariableDeclarationSyntax(
                    new VariableDeclarationSyntax(
                        new VariableDefinitionSyntax(
                            JassSyntaxFactory.ParseTypeName(typeName),
                            new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, name), 0),
                            new EmptyNode(0))),
                    new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0)))));
        }

        public override GlobalDeclarationSyntax? GenerateGlobalDeclaration(string typeName, string name, NewExpressionSyntax value)
        {
            return new GlobalDeclarationSyntax(new GlobalVariableDeclarationSyntax(
                new VariableDeclarationSyntax(
                    new VariableDefinitionSyntax(
                        JassSyntaxFactory.ParseTypeName(typeName),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, name), 0),
                        new EqualsValueClauseSyntax(
                            new TokenNode(new SyntaxToken(SyntaxTokenType.Assignment), 0),
                            value))),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0)))));
        }

        public override FunctionSyntax Build(
            string functionName,
            IEnumerable<(string typeName, string name)> locals,
            IEnumerable<NewStatementSyntax> statements)
        {
            var localDeclarations = locals?.Select(localDeclaration => GenerateLocalDeclaration(localDeclaration.typeName, localDeclaration.name)).ToArray()
                ?? Array.Empty<LocalVariableDeclarationSyntax>();
            return localDeclarations.Length > 0
                ? JassSyntaxFactory.Function(
                    JassSyntaxFactory.FunctionDeclaration(functionName),
                    JassSyntaxFactory.LocalVariableList(localDeclarations),
                    statements.ToArray())
                : JassSyntaxFactory.Function(
                    JassSyntaxFactory.FunctionDeclaration(functionName),
                    statements.ToArray());
        }

        public override FunctionSyntax Build(
            string functionName,
            IEnumerable<(string typeName, string name, NewExpressionSyntax value)> locals,
            IEnumerable<NewStatementSyntax> statements)
        {
            var localDeclarations = locals?.Select(localDeclaration => GenerateLocalDeclaration(localDeclaration.typeName, localDeclaration.name, localDeclaration.value)).ToArray()
                ?? Array.Empty<LocalVariableDeclarationSyntax>();
            return localDeclarations.Length > 0
                ? JassSyntaxFactory.Function(
                    JassSyntaxFactory.FunctionDeclaration(functionName),
                    JassSyntaxFactory.LocalVariableList(localDeclarations),
                    statements.ToArray())
                : JassSyntaxFactory.Function(
                    JassSyntaxFactory.FunctionDeclaration(functionName),
                    statements.ToArray());
        }

        public override IEnumerable<GlobalDeclarationSyntax?> BuildGlobalDeclarations()
        {
            return GlobalDeclarationsGenerator<JassFunctionBuilder, GlobalDeclarationSyntax, FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>.GetGlobals(this);
        }

        public sealed override IEnumerable<FunctionSyntax> BuildMainFunction()
        {
            return Main.MainFunctionGenerator<JassFunctionBuilder, GlobalDeclarationSyntax, FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>.GetFunctions(this);
        }

        public sealed override IEnumerable<FunctionSyntax> BuildConfigFunction()
        {
            return Config.ConfigFunctionGenerator<JassFunctionBuilder, GlobalDeclarationSyntax, FunctionSyntax, NewStatementSyntax, NewExpressionSyntax>.GetFunctions(this);
        }

        protected LocalVariableDeclarationSyntax GenerateLocalDeclaration(string typeName, string name)
        {
            return JassSyntaxFactory.VariableDefinition(JassSyntaxFactory.ParseTypeName(typeName), name);
        }

        protected LocalVariableDeclarationSyntax GenerateLocalDeclaration(string typeName, string name, NewExpressionSyntax value)
        {
            return JassSyntaxFactory.VariableDefinition(JassSyntaxFactory.ParseTypeName(typeName), name, value);
        }

        public override NewStatementSyntax GenerateAssignmentStatement(string variableName, NewExpressionSyntax value)
        {
            return JassSyntaxFactory.SetStatement(variableName, JassSyntaxFactory.EqualsValueClause(value));
        }

        public override NewStatementSyntax GenerateAssignmentStatement(string variableName, NewExpressionSyntax arrayIndex, NewExpressionSyntax value)
        {
            return JassSyntaxFactory.SetStatement(variableName, arrayIndex, JassSyntaxFactory.EqualsValueClause(value));
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

        public override NewStatementSyntax GenerateElseClause(NewStatementSyntax ifStatement, NewExpressionSyntax condition, params NewStatementSyntax[] elseBody)
        {
            var ifNode = ifStatement.StatementNode?.IfStatementNode ?? throw new ArgumentException("Expression node must contain if statement.", nameof(ifStatement));

            var elseClause = condition is null
                ? new ElseClauseSyntax(
                    new ElseSyntax(
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.ElseKeyword), 0),
                        new LineDelimiterSyntax(new EndOfLineSyntax(new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.NewlineSymbol), 0))),
                        new StatementListSyntax(elseBody)))
                : new ElseClauseSyntax(
                    new ElseifSyntax(
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.ElseifKeyword), 0),
                        condition,
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.ThenKeyword), 0),
                        new LineDelimiterSyntax(new EndOfLineSyntax(new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.NewlineSymbol), 0))),
                        new StatementListSyntax(elseBody),
                        new CodeAnalysis.Jass.EmptyNode(0)));

            var elseifs = new Stack<ElseifSyntax>();
            var oldElseClause = ifNode.ElseClauseNode;
            if (oldElseClause != null)
            {
                while (true)
                {
                    if (oldElseClause.ElseNode != null)
                    {
                        throw new ArgumentException("Cannot extend the if statement, because it already contains an else node.", nameof(ifStatement));
                    }

                    var elseif = oldElseClause.ElseifNode;
                    elseifs.Push(elseif);
                    if (elseif.ElseClauseNode != null)
                    {
                        oldElseClause = elseif.ElseClauseNode;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                while (elseifs.Count > 0)
                {
                    var pop = elseifs.Pop();
                    var rebuild = new ElseifSyntax(
                        pop.ElseifKeywordToken,
                        pop.ConditionExpressionNode,
                        pop.ThenKeywordToken,
                        pop.LineDelimiterNode,
                        pop.StatementListNode,
                        elseClause);

                    elseClause = new ElseClauseSyntax(rebuild);
                }
            }

            return new NewStatementSyntax(
                new StatementSyntax(
                    new IfStatementSyntax(
                        ifNode.IfKeywordToken,
                        ifNode.ConditionExpressionNode,
                        ifNode.ThenKeywordToken,
                        ifNode.LineDelimiterNode,
                        ifNode.StatementListNode,
                        elseClause,
                        ifNode.EndifKeywordToken)),
                ifStatement.LineDelimiterNode);
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

        public override NewExpressionSyntax GenerateFloatLiteralExpression(float value, int decimalPlaces)
        {
            // return JassSyntaxFactory.ConstantExpression(value, decimalPlaces);
            return GenerateFloatLiteralExpression(value);
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

        public override NewExpressionSyntax GenerateFunctionReferenceExpression(string functionName)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new FunctionReferenceSyntax(
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.FunctionKeyword), 0),
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.AlphanumericIdentifier, functionName), 0))),
                new CodeAnalysis.Jass.EmptyNode(0));
        }

        public override NewExpressionSyntax GenerateArrayReferenceExpression(string variableName, NewExpressionSyntax index)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new ArrayReferenceSyntax(
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.AlphanumericIdentifier, variableName), 0),
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.SquareBracketOpenSymbol), 0),
                        index,
                        new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(CodeAnalysis.Jass.SyntaxTokenType.SquareBracketCloseSymbol), 0))),
                new CodeAnalysis.Jass.EmptyNode(0));
        }

        public override NewExpressionSyntax GenerateUnaryExpression(UnaryOperator @operator, NewExpressionSyntax expression)
        {
            var operatorTokenType = @operator switch
            {
                UnaryOperator.Not => CodeAnalysis.Jass.SyntaxTokenType.NotOperator,

                _ => throw new ArgumentException($"Unary operator {@operator} is not supported, or not defined", nameof(@operator)),
            };

            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new UnaryExpressionSyntax(
                        new UnaryOperatorSyntax(new CodeAnalysis.Jass.TokenNode(new CodeAnalysis.Jass.SyntaxToken(operatorTokenType), 0)),
                        expression)),
                new CodeAnalysis.Jass.EmptyNode(0));
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

                BinaryOperator.And => CodeAnalysis.Jass.SyntaxTokenType.AndOperator,
                BinaryOperator.Or => CodeAnalysis.Jass.SyntaxTokenType.OrOperator,

                _ => throw new ArgumentException($"Binary operator {@operator} is not supported, or not defined", nameof(@operator)),
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