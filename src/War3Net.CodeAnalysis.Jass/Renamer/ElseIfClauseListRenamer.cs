// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseListRenamer.cs" company="Drake53">
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
        private bool TryRenameElseIfClauseList(ImmutableArray<JassElseIfClauseSyntax> elseIfClauses, [NotNullWhen(true)] out ImmutableArray<JassElseIfClauseSyntax>? renamedElseIfClauses)
        {
            for (var i = 0; i < elseIfClauses.Length; i++)
            {
                if (TryRenameElseIfClause(elseIfClauses[i], out var renamedElseIfClause))
                {
                    var builder = ImmutableArray.CreateBuilder<JassElseIfClauseSyntax>(elseIfClauses.Length);
                    for (var j = 0; j < i; j++)
                    {
                        builder.Add(elseIfClauses[j]);
                    }

                    builder.Add(renamedElseIfClause);

                    while (++i < elseIfClauses.Length)
                    {
                        if (TryRenameElseIfClause(elseIfClauses[i], out renamedElseIfClause))
                        {
                            builder.Add(renamedElseIfClause);
                        }
                        else
                        {
                            builder.Add(elseIfClauses[i]);
                        }
                    }

                    renamedElseIfClauses = builder.ToImmutable();
                    return true;
                }
            }

            renamedElseIfClauses = null;
            return false;
        }
    }
}