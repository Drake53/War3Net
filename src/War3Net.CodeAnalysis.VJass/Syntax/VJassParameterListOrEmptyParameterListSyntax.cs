namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassParameterListOrEmptyParameterListSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassParameterListOrEmptyParameterListSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassParameterListOrEmptyParameterListSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}