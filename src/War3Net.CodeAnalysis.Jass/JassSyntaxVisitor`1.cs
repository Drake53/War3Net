// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxVisitor`1.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// Represents a <see cref="JassSyntaxNode"/> visitor that visits only the single node passed into its <see cref="Visit(JassSyntaxNode?)"/> method
    /// and produces a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the visitor's visit methods.</typeparam>
    public abstract class JassSyntaxVisitor<TResult> : IJassSyntaxVisitor<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JassSyntaxVisitor{TResult}"/> class.
        /// </summary>
        protected JassSyntaxVisitor()
        {
        }

        /// <inheritdoc/>
        public virtual TResult? Visit(JassSyntaxNode? node)
        {
            if (node is not null)
            {
                return node.Accept(this);
            }

            return default;
        }

        /// <inheritdoc/>
        public virtual TResult DefaultVisit(JassSyntaxNode node)
        {
            return default!;
        }

        // Expressions

        /// <inheritdoc/>
        public virtual TResult VisitBinaryExpression(JassBinaryExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitElementAccessExpression(JassElementAccessExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitFunctionReferenceExpression(JassFunctionReferenceExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitIdentifierName(JassIdentifierNameSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitInvocationExpression(JassInvocationExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitLiteralExpression(JassLiteralExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitParenthesizedExpression(JassParenthesizedExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitUnaryExpression(JassUnaryExpressionSyntax node) => DefaultVisit(node);

        // Types

        /// <inheritdoc/>
        public virtual TResult VisitPredefinedType(JassPredefinedTypeSyntax node) => DefaultVisit(node);

        // Statements

        /// <inheritdoc/>
        public virtual TResult VisitCallStatement(JassCallStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitDebugStatement(JassDebugStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitExitStatement(JassExitStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitIfStatement(JassIfStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitLocalVariableDeclarationStatement(JassLocalVariableDeclarationStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitLoopStatement(JassLoopStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitReturnStatement(JassReturnStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitSetStatement(JassSetStatementSyntax node) => DefaultVisit(node);

        // Top-level declarations

        /// <inheritdoc/>
        public virtual TResult VisitCompilationUnit(JassCompilationUnitSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitFunctionDeclaration(JassFunctionDeclarationSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitGlobalsDeclaration(JassGlobalsDeclarationSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitNativeFunctionDeclaration(JassNativeFunctionDeclarationSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitTypeDeclaration(JassTypeDeclarationSyntax node) => DefaultVisit(node);

        // Global declarations

        /// <inheritdoc/>
        public virtual TResult VisitGlobalConstantDeclaration(JassGlobalConstantDeclarationSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitGlobalVariableDeclaration(JassGlobalVariableDeclarationSyntax node) => DefaultVisit(node);

        // Declarators

        /// <inheritdoc/>
        public virtual TResult VisitArrayDeclarator(JassArrayDeclaratorSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitFunctionDeclarator(JassFunctionDeclaratorSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitVariableDeclarator(JassVariableDeclaratorSyntax node) => DefaultVisit(node);

        // Parameter lists

        /// <inheritdoc/>
        public virtual TResult VisitEmptyParameterList(JassEmptyParameterListSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitParameterList(JassParameterListSyntax node) => DefaultVisit(node);

        // Clauses

        /// <inheritdoc/>
        public virtual TResult VisitArgumentList(JassArgumentListSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitElementAccessClause(JassElementAccessClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitElseClause(JassElseClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitElseIfClause(JassElseIfClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitElseIfClauseDeclarator(JassElseIfClauseDeclaratorSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitEqualsValueClause(JassEqualsValueClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitIfClause(JassIfClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitIfClauseDeclarator(JassIfClauseDeclaratorSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitParameter(JassParameterSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual TResult VisitReturnClause(JassReturnClauseSyntax node) => DefaultVisit(node);
    }
}