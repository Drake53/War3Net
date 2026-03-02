namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassTypeSyntax : JassExpressionSyntax
    {
        public abstract JassSyntaxToken Token { get; }

        protected internal override abstract JassTypeSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassTypeSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}