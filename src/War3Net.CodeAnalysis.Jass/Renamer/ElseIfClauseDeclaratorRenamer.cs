// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseDeclaratorRenamer.cs" company="Drake53">
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
        private bool TryRenameElseIfClauseDeclarator(JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator, [NotNullWhen(true)] out JassElseIfClauseDeclaratorSyntax? renamedElseIfClauseDeclarator)
        {
            if (TryRenameExpression(elseIfClauseDeclarator.Condition, out var renamedCondition))
            {
                renamedElseIfClauseDeclarator = new JassElseIfClauseDeclaratorSyntax(
                    elseIfClauseDeclarator.ElseIfToken,
                    renamedCondition,
                    elseIfClauseDeclarator.ThenToken);

                return true;
            }

            renamedElseIfClauseDeclarator = null;
            return false;
        }
    }
}