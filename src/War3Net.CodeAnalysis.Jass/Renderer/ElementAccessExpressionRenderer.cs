using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassElementAccessExpressionSyntax elementAccessExpression)
        {
            Render(elementAccessExpression.IdentifierName);
            Render(elementAccessExpression.ElementAccessClause);
        }
    }
}