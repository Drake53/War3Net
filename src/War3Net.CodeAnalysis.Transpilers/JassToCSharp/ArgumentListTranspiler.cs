namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ArgumentListSyntax Transpile(JassArgumentListSyntax argumentList)
        {
            return SyntaxFactory.ArgumentList(
                Transpile(SyntaxKind.OpenParenToken, argumentList.OpenParenToken),
                SyntaxFactory.SeparatedList(
                    argumentList.Arguments.Items.Select(TranspileArgument),
                    argumentList.Arguments.Separators.Select(Transpile)),
                Transpile(SyntaxKind.CloseParenToken, argumentList.CloseParenToken));
        }
    }
}