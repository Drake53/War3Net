// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxNormalizingVisitor.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// A syntax visitor that normalizes whitespace in JASS syntax trees.
    /// </summary>
    public sealed class JassSyntaxNormalizingVisitor : JassSyntaxVisitor<JassSyntaxNode>
    {
        private static readonly HashSet<JassSyntaxKind> _increaseIndentationSyntaxKinds = GetIncreaseIndentationSyntaxKinds();
        private static readonly HashSet<JassSyntaxKind> _decreaseIndentationSyntaxKinds = GetDecreaseIndentationSyntaxKinds();
        private static readonly HashSet<JassSyntaxKind> _requireNewLineSyntaxKinds = GetRequireNewLineSyntaxKinds();

        private readonly List<JassSyntaxNode> _nodes;
        private readonly bool _addSpacesToOuterInvocation;
        private readonly bool _trimComments;
        private readonly string _indentationString;
        private readonly JassSyntaxTrivia[] _indentationCache;

        private JassSyntaxToken _previousToken;
        private JassSyntaxToken _currentToken;
        private JassSyntaxNode? _previousNode;
        private JassSyntaxNode? _previousNodeParent;
        private JassSyntaxNode? _previousNodeGrandParent;

        private int _currentLevelOfIndentation;
        private bool _encounteredAnyTextOnCurrentLine;
        private bool _requireNewLineTrivia;

        /// <summary>
        /// Initializes a new instance of the <see cref="JassSyntaxNormalizingVisitor"/> class.
        /// </summary>
        /// <param name="addSpacesToOuterInvocation">Whether to add spaces inside parentheses for outer invocations.</param>
        /// <param name="trimComments">Whether to trim trailing whitespace from comments.</param>
        /// <param name="indentationString">The string to use for each level of indentation.</param>
        public JassSyntaxNormalizingVisitor(
            bool addSpacesToOuterInvocation = true,
            bool trimComments = false,
            string indentationString = "    ")
        {
            _nodes = new List<JassSyntaxNode>();
            _addSpacesToOuterInvocation = addSpacesToOuterInvocation;
            _trimComments = trimComments;
            _indentationString = indentationString;
            _indentationCache = BuildIndentationCache(indentationString);

            _previousToken = new JassSyntaxToken(JassSyntaxTriviaList.Empty, JassSyntaxKind.None, string.Empty, JassSyntaxTriviaList.Empty);
            _currentToken = _previousToken;
            _previousNode = null;
            _previousNodeParent = null;
            _previousNodeGrandParent = null;

            _currentLevelOfIndentation = 0;
            _encounteredAnyTextOnCurrentLine = false;
            _requireNewLineTrivia = false;
        }

        /// <summary>
        /// Normalizes whitespace in a compilation unit.
        /// </summary>
        /// <param name="compilationUnit">The compilation unit to normalize.</param>
        /// <returns>The normalized compilation unit.</returns>
        public JassCompilationUnitSyntax NormalizeWhitespace(JassCompilationUnitSyntax compilationUnit)
        {
            return (JassCompilationUnitSyntax)VisitCompilationUnit(compilationUnit);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitCompilationUnit(JassCompilationUnitSyntax node)
        {
            _nodes.Add(node);
            var declarations = VisitList(node.Declarations, VisitTopLevelDeclaration);
            var endOfFileToken = VisitToken(node.EndOfFileToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(declarations, endOfFileToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitFunctionDeclaration(JassFunctionDeclarationSyntax node)
        {
            _nodes.Add(node);
            var functionDeclarator = (JassFunctionDeclaratorSyntax)VisitFunctionDeclarator(node.FunctionDeclarator);

            // After the function declarator, increase indentation for the function body
            _currentLevelOfIndentation++;

            var statements = VisitList(node.Statements, VisitStatement);
            var endFunctionToken = VisitToken(node.EndFunctionToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(functionDeclarator, statements, endFunctionToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitGlobalsDeclaration(JassGlobalsDeclarationSyntax node)
        {
            _nodes.Add(node);
            var globalsToken = VisitToken(node.GlobalsToken);
            var globalDeclarations = VisitList(node.GlobalDeclarations, VisitGlobalDeclaration);
            var endGlobalsToken = VisitToken(node.EndGlobalsToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(globalsToken, globalDeclarations, endGlobalsToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitNativeFunctionDeclaration(JassNativeFunctionDeclarationSyntax node)
        {
            _nodes.Add(node);
            var constantToken = node.ConstantToken is not null ? VisitToken(node.ConstantToken) : null;
            var nativeToken = VisitToken(node.NativeToken);
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            var parameterList = (JassParameterListOrEmptyParameterListSyntax)VisitParameterListOrEmptyParameterList(node.ParameterList);
            var returnClause = (JassReturnClauseSyntax)VisitReturnClause(node.ReturnClause);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(constantToken, nativeToken, identifierName, parameterList, returnClause);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitTypeDeclaration(JassTypeDeclarationSyntax node)
        {
            _nodes.Add(node);
            var typeToken = VisitToken(node.TypeToken);
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            var extendsToken = VisitToken(node.ExtendsToken);
            var baseType = (JassTypeSyntax)Visit(node.BaseType)!;
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(typeToken, identifierName, extendsToken, baseType);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitGlobalConstantDeclaration(JassGlobalConstantDeclarationSyntax node)
        {
            _nodes.Add(node);
            var constantToken = VisitToken(node.ConstantToken);
            var type = (JassTypeSyntax)Visit(node.Type)!;
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            var equalsValueClause = (JassEqualsValueClauseSyntax)VisitEqualsValueClause(node.EqualsValueClause);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(constantToken, type, identifierName, equalsValueClause);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitGlobalVariableDeclaration(JassGlobalVariableDeclarationSyntax node)
        {
            _nodes.Add(node);
            var declarator = (JassVariableOrArrayDeclaratorSyntax)VisitVariableOrArrayDeclarator(node.Declarator);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.WithDeclarator(declarator);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitFunctionDeclarator(JassFunctionDeclaratorSyntax node)
        {
            _nodes.Add(node);
            var constantToken = node.ConstantToken is not null ? VisitToken(node.ConstantToken) : null;
            var functionToken = VisitToken(node.FunctionToken);
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            var parameterList = (JassParameterListOrEmptyParameterListSyntax)VisitParameterListOrEmptyParameterList(node.ParameterList);
            var returnClause = (JassReturnClauseSyntax)VisitReturnClause(node.ReturnClause);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(constantToken, functionToken, identifierName, parameterList, returnClause);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitArrayDeclarator(JassArrayDeclaratorSyntax node)
        {
            _nodes.Add(node);
            var type = (JassTypeSyntax)Visit(node.Type)!;
            var arrayToken = VisitToken(node.ArrayToken);
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(type, arrayToken, identifierName);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitVariableDeclarator(JassVariableDeclaratorSyntax node)
        {
            _nodes.Add(node);
            var type = (JassTypeSyntax)Visit(node.Type)!;
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            var equalsValueClause = node.EqualsValueClause is not null ? (JassEqualsValueClauseSyntax)VisitEqualsValueClause(node.EqualsValueClause) : null;
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(type, identifierName, equalsValueClause);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitEmptyParameterList(JassEmptyParameterListSyntax node)
        {
            _nodes.Add(node);
            var takesToken = VisitToken(node.TakesToken);
            var nothingToken = VisitToken(node.NothingToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(takesToken, nothingToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitParameterList(JassParameterListSyntax node)
        {
            _nodes.Add(node);
            var takesToken = VisitToken(node.TakesToken);
            var parameters = VisitSeparatedList(node.Parameters, p => (JassParameterSyntax)VisitParameter(p));
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(takesToken, parameters);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitParameter(JassParameterSyntax node)
        {
            _nodes.Add(node);
            var type = (JassTypeSyntax)Visit(node.Type)!;
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(type, identifierName);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitReturnClause(JassReturnClauseSyntax node)
        {
            _nodes.Add(node);
            var returnsToken = VisitToken(node.ReturnsToken);
            var returnType = (JassTypeSyntax)Visit(node.ReturnType)!;
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(returnsToken, returnType);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitEqualsValueClause(JassEqualsValueClauseSyntax node)
        {
            _nodes.Add(node);
            var equalsToken = VisitToken(node.EqualsToken);
            var expression = (JassExpressionSyntax)Visit(node.Expression)!;
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(equalsToken, expression);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitArgumentList(JassArgumentListSyntax node)
        {
            _nodes.Add(node);
            var openParenToken = VisitToken(node.OpenParenToken);
            var arguments = VisitSeparatedList(node.Arguments, VisitExpression);
            var closeParenToken = VisitToken(node.CloseParenToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(openParenToken, arguments, closeParenToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitElementAccessClause(JassElementAccessClauseSyntax node)
        {
            _nodes.Add(node);
            var openBracketToken = VisitToken(node.OpenBracketToken);
            var expression = (JassExpressionSyntax)Visit(node.Expression)!;
            var closeBracketToken = VisitToken(node.CloseBracketToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(openBracketToken, expression, closeBracketToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitIfClause(JassIfClauseSyntax node)
        {
            _nodes.Add(node);
            var ifClauseDeclarator = (JassIfClauseDeclaratorSyntax)VisitIfClauseDeclarator(node.IfClauseDeclarator);
            var statements = VisitList(node.Statements, VisitStatement);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(ifClauseDeclarator, statements);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitIfClauseDeclarator(JassIfClauseDeclaratorSyntax node)
        {
            _nodes.Add(node);
            var ifToken = VisitToken(node.IfToken);
            var condition = (JassExpressionSyntax)Visit(node.Condition)!;
            var thenToken = VisitToken(node.ThenToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(ifToken, condition, thenToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitElseIfClause(JassElseIfClauseSyntax node)
        {
            _nodes.Add(node);
            var elseIfClauseDeclarator = (JassElseIfClauseDeclaratorSyntax)VisitElseIfClauseDeclarator(node.ElseIfClauseDeclarator);
            var statements = VisitList(node.Statements, VisitStatement);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(elseIfClauseDeclarator, statements);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitElseIfClauseDeclarator(JassElseIfClauseDeclaratorSyntax node)
        {
            _nodes.Add(node);
            var elseIfToken = VisitToken(node.ElseIfToken);
            var condition = (JassExpressionSyntax)Visit(node.Condition)!;
            var thenToken = VisitToken(node.ThenToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(elseIfToken, condition, thenToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitElseClause(JassElseClauseSyntax node)
        {
            _nodes.Add(node);
            var elseToken = VisitToken(node.ElseToken);
            var statements = VisitList(node.Statements, VisitStatement);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(elseToken, statements);
        }

        // Statements

        /// <inheritdoc/>
        public override JassSyntaxNode VisitCallStatement(JassCallStatementSyntax node)
        {
            _nodes.Add(node);
            var callToken = VisitToken(node.CallToken);
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            var argumentList = (JassArgumentListSyntax)VisitArgumentList(node.ArgumentList);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(callToken, identifierName, argumentList);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitDebugStatement(JassDebugStatementSyntax node)
        {
            _nodes.Add(node);
            var debugToken = VisitToken(node.DebugToken);
            var statement = (JassStatementSyntax)VisitStatement(node.Statement);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(debugToken, statement);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitExitStatement(JassExitStatementSyntax node)
        {
            _nodes.Add(node);
            var exitWhenToken = VisitToken(node.ExitWhenToken);
            var condition = (JassExpressionSyntax)Visit(node.Condition)!;
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(exitWhenToken, condition);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitIfStatement(JassIfStatementSyntax node)
        {
            _nodes.Add(node);
            var ifClause = (JassIfClauseSyntax)VisitIfClause(node.IfClause);
            var elseIfClauses = VisitList(node.ElseIfClauses, n => (JassElseIfClauseSyntax)VisitElseIfClause(n));
            var elseClause = node.ElseClause is not null ? (JassElseClauseSyntax)VisitElseClause(node.ElseClause) : null;
            var endIfToken = VisitToken(node.EndIfToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(ifClause, elseIfClauses, elseClause, endIfToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitLocalVariableDeclarationStatement(JassLocalVariableDeclarationStatementSyntax node)
        {
            _nodes.Add(node);
            var localToken = VisitToken(node.LocalToken);
            var declarator = (JassVariableOrArrayDeclaratorSyntax)VisitVariableOrArrayDeclarator(node.Declarator);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(localToken, declarator);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitLoopStatement(JassLoopStatementSyntax node)
        {
            _nodes.Add(node);
            var loopToken = VisitToken(node.LoopToken);
            var statements = VisitList(node.Statements, VisitStatement);
            var endLoopToken = VisitToken(node.EndLoopToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(loopToken, statements, endLoopToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitReturnStatement(JassReturnStatementSyntax node)
        {
            _nodes.Add(node);
            var returnToken = VisitToken(node.ReturnToken);
            var expression = node.Expression is not null ? (JassExpressionSyntax)Visit(node.Expression) : null;
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(returnToken, expression);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitSetStatement(JassSetStatementSyntax node)
        {
            _nodes.Add(node);
            var setToken = VisitToken(node.SetToken);
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            var elementAccessClause = node.ElementAccessClause is not null ? (JassElementAccessClauseSyntax)VisitElementAccessClause(node.ElementAccessClause) : null;
            var equalsValueClause = (JassEqualsValueClauseSyntax)VisitEqualsValueClause(node.EqualsValueClause);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(setToken, identifierName, elementAccessClause, equalsValueClause);
        }

        // Expressions

        /// <inheritdoc/>
        public override JassSyntaxNode VisitBinaryExpression(JassBinaryExpressionSyntax node)
        {
            _nodes.Add(node);
            var left = (JassExpressionSyntax)Visit(node.Left)!;
            var operatorToken = VisitToken(node.OperatorToken);
            var right = (JassExpressionSyntax)Visit(node.Right)!;
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(left, operatorToken, right);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitElementAccessExpression(JassElementAccessExpressionSyntax node)
        {
            _nodes.Add(node);
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            var elementAccessClause = (JassElementAccessClauseSyntax)VisitElementAccessClause(node.ElementAccessClause);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(identifierName, elementAccessClause);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitFunctionReferenceExpression(JassFunctionReferenceExpressionSyntax node)
        {
            _nodes.Add(node);
            var functionToken = VisitToken(node.FunctionToken);
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(functionToken, identifierName);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitIdentifierName(JassIdentifierNameSyntax node)
        {
            _nodes.Add(node);
            var token = VisitToken(node.Token);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.WithToken(token);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitInvocationExpression(JassInvocationExpressionSyntax node)
        {
            _nodes.Add(node);
            var identifierName = (JassIdentifierNameSyntax)VisitIdentifierName(node.IdentifierName);
            var argumentList = (JassArgumentListSyntax)VisitArgumentList(node.ArgumentList);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(identifierName, argumentList);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitLiteralExpression(JassLiteralExpressionSyntax node)
        {
            _nodes.Add(node);
            var token = VisitToken(node.Token);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.WithToken(token);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitParenthesizedExpression(JassParenthesizedExpressionSyntax node)
        {
            _nodes.Add(node);
            var openParenToken = VisitToken(node.OpenParenToken);
            var expression = (JassExpressionSyntax)Visit(node.Expression)!;
            var closeParenToken = VisitToken(node.CloseParenToken);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(openParenToken, expression, closeParenToken);
        }

        /// <inheritdoc/>
        public override JassSyntaxNode VisitUnaryExpression(JassUnaryExpressionSyntax node)
        {
            _nodes.Add(node);
            var operatorToken = VisitToken(node.OperatorToken);
            var expression = (JassExpressionSyntax)Visit(node.Expression)!;
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.Update(operatorToken, expression);
        }

        // Types

        /// <inheritdoc/>
        public override JassSyntaxNode VisitPredefinedType(JassPredefinedTypeSyntax node)
        {
            _nodes.Add(node);
            var token = VisitToken(node.Token);
            _nodes.RemoveAt(_nodes.Count - 1);

            return node.WithToken(token);
        }

        // Helper methods

        private JassTopLevelDeclarationSyntax VisitTopLevelDeclaration(JassTopLevelDeclarationSyntax node)
        {
            return (JassTopLevelDeclarationSyntax)Visit(node)!;
        }

        private JassGlobalDeclarationSyntax VisitGlobalDeclaration(JassGlobalDeclarationSyntax node)
        {
            return node switch
            {
                JassGlobalConstantDeclarationSyntax constantDeclaration => (JassGlobalConstantDeclarationSyntax)VisitGlobalConstantDeclaration(constantDeclaration),
                JassGlobalVariableDeclarationSyntax variableDeclaration => (JassGlobalVariableDeclarationSyntax)VisitGlobalVariableDeclaration(variableDeclaration),
                _ => node,
            };
        }

        private JassStatementSyntax VisitStatement(JassStatementSyntax node)
        {
            return (JassStatementSyntax)Visit(node)!;
        }

        private JassExpressionSyntax VisitExpression(JassExpressionSyntax node)
        {
            return (JassExpressionSyntax)Visit(node)!;
        }

        private JassParameterListOrEmptyParameterListSyntax VisitParameterListOrEmptyParameterList(JassParameterListOrEmptyParameterListSyntax node)
        {
            return node switch
            {
                JassEmptyParameterListSyntax emptyParameterList => (JassEmptyParameterListSyntax)VisitEmptyParameterList(emptyParameterList),
                JassParameterListSyntax parameterList => (JassParameterListSyntax)VisitParameterList(parameterList),
                _ => node,
            };
        }

        private JassVariableOrArrayDeclaratorSyntax VisitVariableOrArrayDeclarator(JassVariableOrArrayDeclaratorSyntax node)
        {
            return node switch
            {
                JassArrayDeclaratorSyntax arrayDeclarator => (JassArrayDeclaratorSyntax)VisitArrayDeclarator(arrayDeclarator),
                JassVariableDeclaratorSyntax variableDeclarator => (JassVariableDeclaratorSyntax)VisitVariableDeclarator(variableDeclarator),
                _ => node,
            };
        }

        private ImmutableArray<T> VisitList<T>(ImmutableArray<T> list, System.Func<T, T> visitor)
            where T : JassSyntaxNode
        {
            ImmutableArray<T>.Builder? builder = null;

            for (var i = 0; i < list.Length; i++)
            {
                var original = list[i];
                var visited = visitor(original);

                if (builder != null)
                {
                    builder.Add(visited);
                }
                else if (!ReferenceEquals(original, visited))
                {
                    builder = ImmutableArray.CreateBuilder<T>(list.Length);
                    for (var j = 0; j < i; j++)
                    {
                        builder.Add(list[j]);
                    }

                    builder.Add(visited);
                }
            }

            return builder?.MoveToImmutable() ?? list;
        }

        private SeparatedSyntaxList<T, JassSyntaxToken> VisitSeparatedList<T>(SeparatedSyntaxList<T, JassSyntaxToken> list, System.Func<T, T> visitor)
            where T : JassSyntaxNode
        {
            if (list.Items.IsEmpty)
            {
                return list;
            }

            SeparatedSyntaxList<T, JassSyntaxToken>.Builder? builder = null;

            var firstItem = list.Items[0];
            var visitedFirstItem = visitor(firstItem);
            var changed = !ReferenceEquals(firstItem, visitedFirstItem);

            for (var i = 1; i < list.Items.Length; i++)
            {
                var originalSeparator = list.Separators[i - 1];
                var visitedSeparator = VisitToken(originalSeparator);
                var originalItem = list.Items[i];
                var visitedItem = visitor(originalItem);

                if (builder != null)
                {
                    builder.Add(visitedSeparator, visitedItem);
                }
                else if (changed || !ReferenceEquals(originalSeparator, visitedSeparator) || !ReferenceEquals(originalItem, visitedItem))
                {
                    builder = SeparatedSyntaxList<T, JassSyntaxToken>.CreateBuilder(visitedFirstItem, list.Items.Length);
                    for (var j = 1; j < i; j++)
                    {
                        builder.Add(list.Separators[j - 1], list.Items[j]);
                    }

                    builder.Add(visitedSeparator, visitedItem);
                    changed = true;
                }
            }

            if (builder != null)
            {
                return builder.ToSeparatedSyntaxList();
            }

            if (changed)
            {
                return SeparatedSyntaxList<T, JassSyntaxToken>.Create(visitedFirstItem);
            }

            return list;
        }

        // Token and trivia normalization

        private JassSyntaxToken VisitToken(JassSyntaxToken token)
        {
            _currentToken = token;

            if (_decreaseIndentationSyntaxKinds.Contains(_currentToken.SyntaxKind))
            {
                _currentLevelOfIndentation--;
            }

            var leadingTrivia = VisitLeadingTrivia(token.LeadingTrivia);

            if (_requireNewLineSyntaxKinds.Contains(_currentToken.SyntaxKind))
            {
                _requireNewLineTrivia = true;
            }

            if (_increaseIndentationSyntaxKinds.Contains(_currentToken.SyntaxKind))
            {
                _currentLevelOfIndentation++;
            }

            var trailingTrivia = VisitTrailingTrivia(token.TrailingTrivia);

            var result = new JassSyntaxToken(
                leadingTrivia,
                token.SyntaxKind,
                token.Text,
                trailingTrivia);

            _previousToken = result;
            _previousNode = _nodes[^1];
            _previousNodeParent = _nodes.Count > 1 ? _nodes[^2] : null;
            _previousNodeGrandParent = _nodes.Count > 2 ? _nodes[^3] : null;

            return result;
        }

        private JassSyntaxTriviaList VisitLeadingTrivia(JassSyntaxTriviaList triviaList)
        {
            var triviaBuilder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>();

            if (_requireNewLineTrivia)
            {
                triviaBuilder.Add(JassSyntaxTrivia.NewLine);
                _encounteredAnyTextOnCurrentLine = false;
                _requireNewLineTrivia = false;
            }

            HandleExistingTrivia(triviaList, triviaBuilder);

            if (_encounteredAnyTextOnCurrentLine)
            {
                var requireSpace = true;

                if (_previousToken.SyntaxKind == JassSyntaxKind.OpenBracketToken ||
                    _currentToken.SyntaxKind == JassSyntaxKind.OpenBracketToken ||
                    _currentToken.SyntaxKind == JassSyntaxKind.CloseBracketToken ||
                    _currentToken.SyntaxKind == JassSyntaxKind.CommaToken)
                {
                    requireSpace = false;
                }
                else
                {
                    var currentNode = _nodes[^1];
                    if (currentNode is not null)
                    {
                        if (_currentToken.SyntaxKind == JassSyntaxKind.OpenParenToken)
                        {
                            requireSpace = currentNode.SyntaxKind == JassSyntaxKind.ParenthesizedExpression;
                        }
                        else if (_currentToken.SyntaxKind == JassSyntaxKind.CloseParenToken)
                        {
                            if (_addSpacesToOuterInvocation &&
                                currentNode.SyntaxKind == JassSyntaxKind.ArgumentList &&
                                _nodes.Count > 1)
                            {
                                var currentNodeParent = _nodes[^2];

                                if (currentNodeParent.SyntaxKind == JassSyntaxKind.CallStatement)
                                {
                                    requireSpace = true;
                                }
                                else if (currentNodeParent.SyntaxKind == JassSyntaxKind.InvocationExpression && _nodes.Count > 2)
                                {
                                    var currentNodeGrandParent = _nodes[^3];
                                    requireSpace = currentNodeGrandParent.SyntaxKind == JassSyntaxKind.EqualsValueClause;
                                }
                                else
                                {
                                    requireSpace = false;
                                }
                            }
                            else
                            {
                                requireSpace = false;
                            }
                        }
                    }

                    if (_previousNode is not null)
                    {
                        if (_previousNode.SyntaxKind == JassSyntaxKind.UnaryPlusExpression ||
                            _previousNode.SyntaxKind == JassSyntaxKind.UnaryMinusExpression)
                        {
                            requireSpace = false;
                        }
                        else if (_previousToken.SyntaxKind == JassSyntaxKind.OpenParenToken)
                        {
                            if (_addSpacesToOuterInvocation &&
                                _previousNode.SyntaxKind == JassSyntaxKind.ArgumentList &&
                                _previousNodeParent is not null)
                            {
                                if (_previousNodeParent.SyntaxKind == JassSyntaxKind.CallStatement)
                                {
                                    requireSpace = true;
                                    if (_currentToken.SyntaxKind == JassSyntaxKind.CloseParenToken)
                                    {
                                        requireSpace = false;
                                        triviaBuilder.Add(JassSyntaxFactory.WhitespaceTrivia("  "));
                                    }
                                }
                                else if (_previousNodeParent.SyntaxKind == JassSyntaxKind.InvocationExpression &&
                                         _previousNodeGrandParent is not null &&
                                         _previousNodeGrandParent.SyntaxKind == JassSyntaxKind.EqualsValueClause)
                                {
                                    requireSpace = true;
                                    if (_currentToken.SyntaxKind == JassSyntaxKind.CloseParenToken)
                                    {
                                        requireSpace = false;
                                        triviaBuilder.Add(JassSyntaxFactory.WhitespaceTrivia("  "));
                                    }
                                }
                                else
                                {
                                    requireSpace = false;
                                }
                            }
                            else
                            {
                                requireSpace = false;
                            }
                        }
                    }
                }

                if (requireSpace)
                {
                    triviaBuilder.Add(JassSyntaxTrivia.SingleSpace);
                }
            }
            else if (!string.IsNullOrEmpty(_currentToken.Text))
            {
                _encounteredAnyTextOnCurrentLine = true;
                if (_currentLevelOfIndentation > 0)
                {
                    triviaBuilder.Add(GetIndentationTrivia());
                }
            }

            return JassSyntaxFactory.SyntaxTriviaList(triviaBuilder.ToImmutable());
        }

        private JassSyntaxTriviaList VisitTrailingTrivia(JassSyntaxTriviaList triviaList)
        {
            var triviaBuilder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>();

            HandleExistingTrivia(triviaList, triviaBuilder);

            return JassSyntaxFactory.SyntaxTriviaList(triviaBuilder.ToImmutable());
        }

        private void HandleExistingTrivia(JassSyntaxTriviaList triviaList, ImmutableArray<JassSyntaxTrivia>.Builder triviaBuilder)
        {
            for (var i = 0; i < triviaList.Trivia.Length; i++)
            {
                var trivia = triviaList.Trivia[i];
                if (trivia.SyntaxKind == JassSyntaxKind.NewLineTrivia)
                {
                    triviaBuilder.Add(trivia);
                    _encounteredAnyTextOnCurrentLine = false;
                    _requireNewLineTrivia = false;
                }
                else if (trivia.SyntaxKind == JassSyntaxKind.SingleLineCommentTrivia)
                {
                    if (!_encounteredAnyTextOnCurrentLine)
                    {
                        _encounteredAnyTextOnCurrentLine = true;
                        if (_currentLevelOfIndentation > 0)
                        {
                            triviaBuilder.Add(GetIndentationTrivia());
                        }
                    }
                    else if (_previousToken.TrailingTrivia.Trivia.IsEmpty || _previousToken.TrailingTrivia.Trivia[^1].SyntaxKind != JassSyntaxKind.WhitespaceTrivia)
                    {
                        triviaBuilder.Add(JassSyntaxTrivia.SingleSpace);
                    }

                    if (_trimComments && char.IsWhiteSpace(trivia.Text[^1]))
                    {
                        triviaBuilder.Add(JassSyntaxFactory.SingleLineCommentTrivia(trivia.Text.TrimEnd()));
                    }
                    else
                    {
                        triviaBuilder.Add(trivia);
                    }

                    _requireNewLineTrivia = true;
                }
            }

            if (_requireNewLineTrivia)
            {
                triviaBuilder.Add(JassSyntaxTrivia.NewLine);
                _encounteredAnyTextOnCurrentLine = false;
                _requireNewLineTrivia = false;
            }
        }

        private static HashSet<JassSyntaxKind> GetIncreaseIndentationSyntaxKinds()
        {
            return new HashSet<JassSyntaxKind>
            {
                JassSyntaxKind.ElseKeyword,
                JassSyntaxKind.GlobalsKeyword,
                JassSyntaxKind.LoopKeyword,
                JassSyntaxKind.ThenKeyword,
            };
        }

        private static HashSet<JassSyntaxKind> GetDecreaseIndentationSyntaxKinds()
        {
            return new HashSet<JassSyntaxKind>
            {
                JassSyntaxKind.ElseIfKeyword,
                JassSyntaxKind.ElseKeyword,
                JassSyntaxKind.EndFunctionKeyword,
                JassSyntaxKind.EndGlobalsKeyword,
                JassSyntaxKind.EndIfKeyword,
                JassSyntaxKind.EndLoopKeyword,
            };
        }

        private static HashSet<JassSyntaxKind> GetRequireNewLineSyntaxKinds()
        {
            return new HashSet<JassSyntaxKind>
            {
                JassSyntaxKind.ElseKeyword,
                JassSyntaxKind.EndFunctionKeyword,
                JassSyntaxKind.EndGlobalsKeyword,
                JassSyntaxKind.EndIfKeyword,
                JassSyntaxKind.EndLoopKeyword,
                JassSyntaxKind.GlobalsKeyword,
                JassSyntaxKind.LoopKeyword,
                JassSyntaxKind.ThenKeyword,
            };
        }

        private static JassSyntaxTrivia[] BuildIndentationCache(string indentationString)
        {
            const int maxCachedDepth = 4;
            var cache = new JassSyntaxTrivia[maxCachedDepth];
            var sb = new System.Text.StringBuilder(indentationString);
            for (var i = 0; i < maxCachedDepth; i++)
            {
                cache[i] = JassSyntaxFactory.WhitespaceTrivia(sb.ToString());
                sb.Append(indentationString);
            }

            return cache;
        }

        private JassSyntaxTrivia GetIndentationTrivia()
        {
            var index = _currentLevelOfIndentation - 1;
            return index < _indentationCache.Length
                ? _indentationCache[index]
                : JassSyntaxFactory.WhitespaceTrivia(string.Concat(System.Linq.Enumerable.Repeat(_indentationString, _currentLevelOfIndentation)));
        }
    }
}