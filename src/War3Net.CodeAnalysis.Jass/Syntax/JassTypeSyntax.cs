namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassTypeSyntax : JassExpressionSyntax
    {
        protected internal override abstract JassTypeSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassTypeSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}