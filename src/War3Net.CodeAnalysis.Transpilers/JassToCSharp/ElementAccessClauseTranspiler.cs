namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public BracketedArgumentListSyntax Transpile(JassElementAccessClauseSyntax elementAccessClause)
        {
            return SyntaxFactory.BracketedArgumentList(
                Transpile(SyntaxKind.OpenBracketToken, elementAccessClause.OpenBracketToken),
                SyntaxFactory.SingletonSeparatedList(TranspileArgument(elementAccessClause.Argument)),
                Transpile(SyntaxKind.CloseBracketToken, elementAccessClause.CloseBracketToken));
        }
    }
}