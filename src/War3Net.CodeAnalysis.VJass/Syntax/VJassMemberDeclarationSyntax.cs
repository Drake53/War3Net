namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassMemberDeclarationSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassMemberDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassMemberDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}