// ------------------------------------------------------------------------------
// <copyright file="IfClauseDeclaratorRenamer.cs" company="Drake53">
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
        private bool TryRenameIfClauseDeclarator(JassIfClauseDeclaratorSyntax ifClauseDeclarator, [NotNullWhen(true)] out JassIfClauseDeclaratorSyntax? renamedIfClauseDeclarator)
        {
            if (TryRenameExpression(ifClauseDeclarator.Condition, out var renamedCondition))
            {
                renamedIfClauseDeclarator = new JassIfClauseDeclaratorSyntax(
                    ifClauseDeclarator.IfToken,
                    renamedCondition,
                    ifClauseDeclarator.ThenToken);

                return true;
            }

            renamedIfClauseDeclarator = null;
            return false;
        }
    }
}