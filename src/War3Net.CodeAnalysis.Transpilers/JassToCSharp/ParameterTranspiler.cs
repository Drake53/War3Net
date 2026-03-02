namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ParameterSyntax Transpile(JassParameterSyntax parameter)
        {
            return SyntaxFactory.Parameter(
                default,
                SyntaxFactory.TokenList(),
                Transpile(parameter.Type),
                Transpile(parameter.IdentifierName.Token),
                null);
        }
    }
}