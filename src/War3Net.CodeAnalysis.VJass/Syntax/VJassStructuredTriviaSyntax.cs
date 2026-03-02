namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassStructuredTriviaSyntax : VJassSyntaxNode, ISyntaxTrivia
    {
        protected internal override abstract VJassStructuredTriviaSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassStructuredTriviaSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}