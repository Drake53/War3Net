namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassParameterListOrEmptyParameterListSyntax : JassSyntaxNode
    {
        public abstract JassSyntaxToken TakesToken { get; }

        public abstract SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> Parameters { get; }

        protected internal override abstract JassParameterListOrEmptyParameterListSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassParameterListOrEmptyParameterListSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}