// ------------------------------------------------------------------------------
// <copyright file="IfStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassIfStatementSyntax ifStatement)
        {
            ElseClauseSyntax? elseClause = null;
            if (ifStatement.ElseClause is not null)
            {
                elseClause = Transpile(ifStatement.ElseClause);
            }

            foreach (var elseIfClause in ifStatement.ElseIfClauses.Reverse())
            {
                elseClause = SyntaxFactory.ElseClause(Transpile(elseIfClause, elseClause));
            }

            return SyntaxFactory.IfStatement(
                SyntaxFactory.List<AttributeListSyntax>(),
                Transpile(ifStatement.IfClause.IfClauseDeclarator.Condition),
                SyntaxFactory.Block(Transpile(ifStatement.IfClause.Statements)),
                elseClause);
        }
    }
}