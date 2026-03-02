namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassScopedDeclarationSyntax : VJassTopLevelDeclarationSyntax
    {
        protected internal override abstract VJassScopedDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassScopedDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}