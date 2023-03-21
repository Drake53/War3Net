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
        private bool TryRenameStatementList(ImmutableArray<JassStatementSyntax> statements, [NotNullWhen(true)] out ImmutableArray<JassStatementSyntax>? renamedStatements)
        {
            for (var i = 0; i < statements.Length; i++)
            {
                if (TryRenameStatement(statements[i], out var renamedStatement))
                {
                    var builder = ImmutableArray.CreateBuilder<JassStatementSyntax>(statements.Length);
                    for (var j = 0; j < i; j++)
                    {
                        builder.Add(statements[j]);
                    }

                    builder.Add(renamedStatement);

                    while (++i < statements.Length)
                    {
                        if (TryRenameStatement(statements[i], out renamedStatement))
                        {
                            builder.Add(renamedStatement);
                        }
                        else
                        {
                            builder.Add(statements[i]);
                        }
                    }

                    renamedStatements = builder.ToImmutable();
                    return true;
                }
            }

            renamedStatements = null;
            return false;
        }
    }
}