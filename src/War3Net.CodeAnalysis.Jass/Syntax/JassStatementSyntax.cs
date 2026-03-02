namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassStatementSyntax : JassSyntaxNode
    {
        protected internal override abstract JassStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassStatementSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}