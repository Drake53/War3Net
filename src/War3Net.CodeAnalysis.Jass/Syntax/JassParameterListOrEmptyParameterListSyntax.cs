namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassParameterListOrEmptyParameterListSyntax : JassSyntaxNode
    {
        protected internal override abstract JassParameterListOrEmptyParameterListSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassParameterListOrEmptyParameterListSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}