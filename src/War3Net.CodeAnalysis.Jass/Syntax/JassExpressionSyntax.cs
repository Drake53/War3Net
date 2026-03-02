namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassExpressionSyntax : JassSyntaxNode
    {
        protected internal override abstract JassExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}