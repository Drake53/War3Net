namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassExpressionSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}