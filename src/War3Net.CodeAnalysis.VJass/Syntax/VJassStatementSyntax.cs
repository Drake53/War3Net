namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassStatementSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}