namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// Represents a visitor that visits only the single <see cref="JassSyntaxNode"/> passed into its <see cref="Visit(JassSyntaxNode?)"/> method
    /// and produces a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the visitor's visit methods.</typeparam>
    public interface IJassSyntaxVisitor<out TResult>
    {
        /// <summary>
        /// Called when visiting a syntax node. Dispatches to the specific visit method for the node type.
        /// </summary>
        TResult? Visit(JassSyntaxNode? node);

        /// <summary>
        /// Called when visiting a node that does not have a specific visit method.
        /// </summary>
        TResult DefaultVisit(JassSyntaxNode node);

        // Expressions
        TResult VisitBinaryExpression(JassBinaryExpressionSyntax node);

        TResult VisitElementAccessExpression(JassElementAccessExpressionSyntax node);

        TResult VisitFunctionReferenceExpression(JassFunctionReferenceExpressionSyntax node);

        TResult VisitIdentifierName(JassIdentifierNameSyntax node);

        TResult VisitInvocationExpression(JassInvocationExpressionSyntax node);

        TResult VisitLiteralExpression(JassLiteralExpressionSyntax node);

        TResult VisitParenthesizedExpression(JassParenthesizedExpressionSyntax node);

        TResult VisitUnaryExpression(JassUnaryExpressionSyntax node);

        // Types
        TResult VisitPredefinedType(JassPredefinedTypeSyntax node);

        // Statements
        TResult VisitCallStatement(JassCallStatementSyntax node);

        TResult VisitDebugStatement(JassDebugStatementSyntax node);

        TResult VisitExitStatement(JassExitStatementSyntax node);

        TResult VisitIfStatement(JassIfStatementSyntax node);

        TResult VisitLocalVariableDeclarationStatement(JassLocalVariableDeclarationStatementSyntax node);

        TResult VisitLoopStatement(JassLoopStatementSyntax node);

        TResult VisitReturnStatement(JassReturnStatementSyntax node);

        TResult VisitSetStatement(JassSetStatementSyntax node);

        // Top-level declarations
        TResult VisitCompilationUnit(JassCompilationUnitSyntax node);

        TResult VisitFunctionDeclaration(JassFunctionDeclarationSyntax node);

        TResult VisitGlobalsDeclaration(JassGlobalsDeclarationSyntax node);

        TResult VisitNativeFunctionDeclaration(JassNativeFunctionDeclarationSyntax node);

        TResult VisitTypeDeclaration(JassTypeDeclarationSyntax node);

        // Global declarations
        TResult VisitGlobalConstantDeclaration(JassGlobalConstantDeclarationSyntax node);

        TResult VisitGlobalVariableDeclaration(JassGlobalVariableDeclarationSyntax node);

        // Declarators
        TResult VisitArrayDeclarator(JassArrayDeclaratorSyntax node);

        TResult VisitFunctionDeclarator(JassFunctionDeclaratorSyntax node);

        TResult VisitVariableDeclarator(JassVariableDeclaratorSyntax node);

        // Parameter lists
        TResult VisitEmptyParameterList(JassEmptyParameterListSyntax node);

        TResult VisitParameterList(JassParameterListSyntax node);

        // Clauses
        TResult VisitArgumentList(JassArgumentListSyntax node);

        TResult VisitElementAccessClause(JassElementAccessClauseSyntax node);

        TResult VisitElseClause(JassElseClauseSyntax node);

        TResult VisitElseIfClause(JassElseIfClauseSyntax node);

        TResult VisitElseIfClauseDeclarator(JassElseIfClauseDeclaratorSyntax node);

        TResult VisitEqualsValueClause(JassEqualsValueClauseSyntax node);

        TResult VisitIfClause(JassIfClauseSyntax node);

        TResult VisitIfClauseDeclarator(JassIfClauseDeclaratorSyntax node);

        TResult VisitParameter(JassParameterSyntax node);

        TResult VisitReturnClause(JassReturnClauseSyntax node);
    }
}