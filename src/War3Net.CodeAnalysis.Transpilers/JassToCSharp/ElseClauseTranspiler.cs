using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ElseClauseSyntax Transpile(JassElseClauseSyntax elseClause, JassSyntaxToken closingToken)
        {
            var elseBlock = SyntaxFactory.Block(
                Transpile(SyntaxKind.OpenBraceToken, elseClause.ElseToken.TrailingTrivia),
                SyntaxFactory.List(elseClause.Statements.Select(Transpile)),
                Transpile(SyntaxKind.CloseBraceToken, closingToken));

            return SyntaxFactory.ElseClause(
                Token(SyntaxKind.ElseKeyword, SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                elseBlock);
        }
    }
}