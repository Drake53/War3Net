namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassTopLevelDeclarationSyntax : JassSyntaxNode
    {
        protected internal override abstract JassTopLevelDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassTopLevelDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}