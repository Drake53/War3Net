using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ArgumentListSyntax Transpile(JassArgumentListSyntax argumentList)
        {
            return SyntaxFactory.ArgumentList(
                Transpile(SyntaxKind.OpenParenToken, argumentList.OpenParenToken),
                SyntaxFactory.SeparatedList(
                    argumentList.ArgumentList.Items.Select(TranspileArgument),
                    argumentList.ArgumentList.Separators.Select(Transpile)),
                Transpile(SyntaxKind.CloseParenToken, argumentList.CloseParenToken));
        }
    }
}