namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassTopLevelDeclarationSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassTopLevelDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassTopLevelDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}