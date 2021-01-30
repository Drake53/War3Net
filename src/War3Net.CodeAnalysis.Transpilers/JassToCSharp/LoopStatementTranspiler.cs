// ------------------------------------------------------------------------------
// <copyright file="LoopStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
                SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression),
                SyntaxFactory.Block(Transpile(loopStatement.Body)));
        }
    }
}