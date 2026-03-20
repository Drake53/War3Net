namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// Represents a <see cref="JassSyntaxNode"/> visitor that visits only the single node passed into its <see cref="Visit(JassSyntaxNode?)"/> method.
    /// </summary>
    public abstract class JassSyntaxVisitor : IJassSyntaxVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JassSyntaxVisitor"/> class.
        /// </summary>
        protected JassSyntaxVisitor()
        {
        }

        /// <inheritdoc/>
        public virtual void Visit(JassSyntaxNode? node)
        {
            node?.Accept(this);
        }

        /// <inheritdoc/>
        public virtual void DefaultVisit(JassSyntaxNode node)
        {
        }

        // Expressions

        /// <inheritdoc/>
        public virtual void VisitBinaryExpression(JassBinaryExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitElementAccessExpression(JassElementAccessExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitFunctionReferenceExpression(JassFunctionReferenceExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitIdentifierName(JassIdentifierNameSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitInvocationExpression(JassInvocationExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitLiteralExpression(JassLiteralExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitParenthesizedExpression(JassParenthesizedExpressionSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitUnaryExpression(JassUnaryExpressionSyntax node) => DefaultVisit(node);

        // Types

        /// <inheritdoc/>
        public virtual void VisitPredefinedType(JassPredefinedTypeSyntax node) => DefaultVisit(node);

        // Statements

        /// <inheritdoc/>
        public virtual void VisitCallStatement(JassCallStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitDebugStatement(JassDebugStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitExitStatement(JassExitStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitIfStatement(JassIfStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitLocalVariableDeclarationStatement(JassLocalVariableDeclarationStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitLoopStatement(JassLoopStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitReturnStatement(JassReturnStatementSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitSetStatement(JassSetStatementSyntax node) => DefaultVisit(node);

        // Top-level declarations

        /// <inheritdoc/>
        public virtual void VisitCompilationUnit(JassCompilationUnitSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitFunctionDeclaration(JassFunctionDeclarationSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitGlobalsDeclaration(JassGlobalsDeclarationSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitNativeFunctionDeclaration(JassNativeFunctionDeclarationSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitTypeDeclaration(JassTypeDeclarationSyntax node) => DefaultVisit(node);

        // Global declarations

        /// <inheritdoc/>
        public virtual void VisitGlobalConstantDeclaration(JassGlobalConstantDeclarationSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitGlobalVariableDeclaration(JassGlobalVariableDeclarationSyntax node) => DefaultVisit(node);

        // Declarators

        /// <inheritdoc/>
        public virtual void VisitArrayDeclarator(JassArrayDeclaratorSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitFunctionDeclarator(JassFunctionDeclaratorSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitVariableDeclarator(JassVariableDeclaratorSyntax node) => DefaultVisit(node);

        // Parameter lists

        /// <inheritdoc/>
        public virtual void VisitEmptyParameterList(JassEmptyParameterListSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitParameterList(JassParameterListSyntax node) => DefaultVisit(node);

        // Clauses

        /// <inheritdoc/>
        public virtual void VisitArgumentList(JassArgumentListSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitElementAccessClause(JassElementAccessClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitElseClause(JassElseClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitElseIfClause(JassElseIfClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitElseIfClauseDeclarator(JassElseIfClauseDeclaratorSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitEqualsValueClause(JassEqualsValueClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitIfClause(JassIfClauseSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitIfClauseDeclarator(JassIfClauseDeclaratorSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitParameter(JassParameterSyntax node) => DefaultVisit(node);

        /// <inheritdoc/>
        public virtual void VisitReturnClause(JassReturnClauseSyntax node) => DefaultVisit(node);
    }
}