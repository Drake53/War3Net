// ------------------------------------------------------------------------------
// <copyright file="LoopStatementTranspiler.cs" company="Drake53">
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
        public StatementSyntax Transpile(JassLoopStatementSyntax loopStatement)
        {
            return SyntaxFactory.WhileStatement(
                Transpile(
                    loopStatement.LoopToken.LeadingTrivia,
                    SyntaxKind.WhileKeyword,
                    JassSyntaxTriviaList.SingleSpace),
                SyntaxFactory.Token(SyntaxKind.OpenParenToken),
                SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression),
                SyntaxFactory.Token(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.CloseParenToken,
                    SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                SyntaxFactory.Block(
                    Transpile(SyntaxKind.OpenBraceToken, loopStatement.LoopToken.TrailingTrivia),
                    SyntaxFactory.List(loopStatement.Statements.Select(Transpile)),
                    Transpile(SyntaxKind.CloseBraceToken, loopStatement.EndLoopToken)));
        }
    }
}