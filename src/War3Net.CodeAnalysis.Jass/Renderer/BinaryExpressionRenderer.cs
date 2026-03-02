using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassBinaryExpressionSyntax binaryExpression)
        {
            Render(binaryExpression.Left);
            WriteSpace();
            Render(binaryExpression.OperatorToken);
            WriteSpace();
            Render(binaryExpression.Right);
        }
    }
}