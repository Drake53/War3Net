// ------------------------------------------------------------------------------
// <copyright file="IfStatementRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameIfStatement(JassIfStatementSyntax ifStatement, [NotNullWhen(true)] out IStatementSyntax? renamedIfStatement)
        {
            var isRenamed = false;

            var elseIfClausesBuilder = ImmutableArray.CreateBuilder<JassElseIfClauseSyntax>();
            foreach (var elseIfClause in ifStatement.ElseIfClauses)
            {
                if (TryRenameElseIfClause(elseIfClause, out var renamedElseIfClause))
                {
                    elseIfClausesBuilder.Add(renamedElseIfClause);
                    isRenamed = true;
                }
                else
                {
                    elseIfClausesBuilder.Add(elseIfClause);
                }
            }

            if (TryRenameExpression(ifStatement.Condition, out var renamedCondition) |
                TryRenameStatementList(ifStatement.Body, out var renamedBody) |
                isRenamed |
                TryRenameElseClause(ifStatement.ElseClause, out var renamedElseClause))
            {
                renamedIfStatement = new JassIfStatementSyntax(
                    renamedCondition ?? ifStatement.Condition,
                    renamedBody ?? ifStatement.Body,
                    isRenamed ? elseIfClausesBuilder.ToImmutable() : ifStatement.ElseIfClauses,
                    renamedElseClause ?? ifStatement.ElseClause);

                return true;
            }

            renamedIfStatement = null;
            return false;
        }
    }
}