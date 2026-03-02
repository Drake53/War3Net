namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public EqualsValueClauseSyntax Transpile(JassEqualsValueClauseSyntax equalsValueClause)
        {
            return SyntaxFactory.EqualsValueClause(
                Transpile(SyntaxKind.EqualsToken, equalsValueClause.EqualsToken),
                Transpile(equalsValueClause.Value));
        }
    }
}