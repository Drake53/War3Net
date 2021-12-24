// ------------------------------------------------------------------------------
// <copyright file="StatementListRenamer.cs" company="Drake53">
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
        private bool TryRenameStatementList(JassStatementListSyntax statementList, [NotNullWhen(true)] out JassStatementListSyntax? renamedStatementList)
        {
            var isRenamed = false;

            var statementsBuilder = ImmutableArray.CreateBuilder<IStatementSyntax>();
            foreach (var statement in statementList.Statements)
            {
                if (TryRenameStatement(statement, out var renamedStatement))
                {
                    statementsBuilder.Add(renamedStatement);
                    isRenamed = true;
                }
                else
                {
                    statementsBuilder.Add(statement);
                }
            }

            if (isRenamed)
            {
                renamedStatementList = new JassStatementListSyntax(statementsBuilder.ToImmutable());
                return true;
            }

            renamedStatementList = null;
            return false;
        }
    }
}