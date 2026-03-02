namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassGlobalDeclarationSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassGlobalDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassGlobalDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}