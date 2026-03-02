namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassScopedGlobalDeclarationSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassScopedGlobalDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassScopedGlobalDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}