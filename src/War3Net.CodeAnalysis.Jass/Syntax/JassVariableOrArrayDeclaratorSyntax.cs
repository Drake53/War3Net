namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassVariableOrArrayDeclaratorSyntax : JassSyntaxNode
    {
        protected internal override abstract JassVariableOrArrayDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassVariableOrArrayDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}