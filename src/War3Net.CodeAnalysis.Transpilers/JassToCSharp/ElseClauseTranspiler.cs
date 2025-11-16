// ------------------------------------------------------------------------------
// <copyright file="ElseClauseTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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