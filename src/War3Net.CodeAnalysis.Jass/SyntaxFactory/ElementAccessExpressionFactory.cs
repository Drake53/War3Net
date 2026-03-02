namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassElementAccessExpressionSyntax ElementAccessExpression(JassIdentifierNameSyntax identifierName, JassElementAccessClauseSyntax elementAccessClause)
        {
            return new JassElementAccessExpressionSyntax(
                identifierName,
                elementAccessClause);
        }

        public static JassElementAccessExpressionSyntax ElementAccessExpression(JassIdentifierNameSyntax identifierName, JassExpressionSyntax elementAccessClauseExpression)
        {
            return new JassElementAccessExpressionSyntax(
                identifierName,
                ElementAccessClause(elementAccessClauseExpression));
        }

        public static JassElementAccessExpressionSyntax ElementAccessExpression(string name, JassElementAccessClauseSyntax elementAccessClause)
        {
            return new JassElementAccessExpressionSyntax(
                ParseIdentifierName(name),
                elementAccessClause);
        }

        public static JassElementAccessExpressionSyntax ElementAccessExpression(string name, JassExpressionSyntax elementAccessClauseExpression)
        {
            return new JassElementAccessExpressionSyntax(
                ParseIdentifierName(name),
                ElementAccessClause(elementAccessClauseExpression));
        }
    }
}