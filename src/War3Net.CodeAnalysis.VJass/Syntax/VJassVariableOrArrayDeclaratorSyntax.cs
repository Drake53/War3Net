namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassVariableOrArrayDeclaratorSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassVariableOrArrayDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassVariableOrArrayDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}