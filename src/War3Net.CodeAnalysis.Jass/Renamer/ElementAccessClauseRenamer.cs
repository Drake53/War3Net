// ------------------------------------------------------------------------------
// <copyright file="ElementAccessClauseRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameElementAccessClause(JassElementAccessClauseSyntax? elementAccessClause, [NotNullWhen(true)] out JassElementAccessClauseSyntax? renamedElementAccessClause)
        {
            if (elementAccessClause is not null &&
                TryRenameExpression(elementAccessClause.Expression, out var renamedExpression))
            {
                renamedElementAccessClause = new JassElementAccessClauseSyntax(
                    elementAccessClause.OpenBracketToken,
                    renamedExpression,
                    elementAccessClause.CloseBracketToken);

                return true;
            }

            renamedElementAccessClause = null;
            return false;
        }
    }
}