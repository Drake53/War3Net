namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassVariableOrArrayDeclaratorSyntax : JassSyntaxNode
    {
        public abstract JassTypeSyntax Type { get; }

        public abstract JassIdentifierNameSyntax IdentifierName { get; }

        protected internal override abstract JassVariableOrArrayDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassVariableOrArrayDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}