namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassGlobalDeclarationSyntax : JassSyntaxNode
    {
        protected internal override abstract JassGlobalDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassGlobalDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}