namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassMethodOrOperatorDeclaratorSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassMethodOrOperatorDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassMethodOrOperatorDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}