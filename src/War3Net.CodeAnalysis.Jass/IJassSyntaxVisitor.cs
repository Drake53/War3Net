namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// Represents a visitor that visits only the single <see cref="JassSyntaxNode"/> passed into its <see cref="Visit(JassSyntaxNode?)"/> method.
    /// </summary>
    public interface IJassSyntaxVisitor
    {
        /// <summary>
        /// Called when visiting a syntax node. Dispatches to the specific visit method for the node type.
        /// </summary>
        void Visit(JassSyntaxNode? node);

        /// <summary>
        /// Called when visiting a node that does not have a specific visit method.
        /// </summary>
        void DefaultVisit(JassSyntaxNode node);

        // Expressions
        void VisitBinaryExpression(JassBinaryExpressionSyntax node);

        void VisitElementAccessExpression(JassElementAccessExpressionSyntax node);

        void VisitFunctionReferenceExpression(JassFunctionReferenceExpressionSyntax node);

        void VisitIdentifierName(JassIdentifierNameSyntax node);

        void VisitInvocationExpression(JassInvocationExpressionSyntax node);

        void VisitLiteralExpression(JassLiteralExpressionSyntax node);

        void VisitParenthesizedExpression(JassParenthesizedExpressionSyntax node);

        void VisitUnaryExpression(JassUnaryExpressionSyntax node);

        // Types
        void VisitPredefinedType(JassPredefinedTypeSyntax node);

        // Statements
        void VisitCallStatement(JassCallStatementSyntax node);

        void VisitDebugStatement(JassDebugStatementSyntax node);

        void VisitExitStatement(JassExitStatementSyntax node);

        void VisitIfStatement(JassIfStatementSyntax node);

        void VisitLocalVariableDeclarationStatement(JassLocalVariableDeclarationStatementSyntax node);

        void VisitLoopStatement(JassLoopStatementSyntax node);

        void VisitReturnStatement(JassReturnStatementSyntax node);

        void VisitSetStatement(JassSetStatementSyntax node);

        // Top-level declarations
        void VisitCompilationUnit(JassCompilationUnitSyntax node);

        void VisitFunctionDeclaration(JassFunctionDeclarationSyntax node);

        void VisitGlobalsDeclaration(JassGlobalsDeclarationSyntax node);

        void VisitNativeFunctionDeclaration(JassNativeFunctionDeclarationSyntax node);

        void VisitTypeDeclaration(JassTypeDeclarationSyntax node);

        // Global declarations
        void VisitGlobalConstantDeclaration(JassGlobalConstantDeclarationSyntax node);

        void VisitGlobalVariableDeclaration(JassGlobalVariableDeclarationSyntax node);

        // Declarators
        void VisitArrayDeclarator(JassArrayDeclaratorSyntax node);

        void VisitFunctionDeclarator(JassFunctionDeclaratorSyntax node);

        void VisitVariableDeclarator(JassVariableDeclaratorSyntax node);

        // Parameter lists
        void VisitEmptyParameterList(JassEmptyParameterListSyntax node);

        void VisitParameterList(JassParameterListSyntax node);

        // Clauses
        void VisitArgumentList(JassArgumentListSyntax node);

        void VisitElementAccessClause(JassElementAccessClauseSyntax node);

        void VisitElseClause(JassElseClauseSyntax node);

        void VisitElseIfClause(JassElseIfClauseSyntax node);

        void VisitElseIfClauseDeclarator(JassElseIfClauseDeclaratorSyntax node);

        void VisitEqualsValueClause(JassEqualsValueClauseSyntax node);

        void VisitIfClause(JassIfClauseSyntax node);

        void VisitIfClauseDeclarator(JassIfClauseDeclaratorSyntax node);

        void VisitParameter(JassParameterSyntax node);

        void VisitReturnClause(JassReturnClauseSyntax node);
    }
}