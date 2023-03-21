// ------------------------------------------------------------------------------
// <copyright file="ArrayDeclaratorRenamer.cs" company="Drake53">
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
        private bool TryRenameArrayDeclarator(JassArrayDeclaratorSyntax arrayDeclarator, [NotNullWhen(true)] out JassVariableOrArrayDeclaratorSyntax? renamedArrayDeclarator)
        {
            if (TryRenameVariableIdentifierName(arrayDeclarator.IdentifierName, out var renamedIdentifierName))
            {
                renamedArrayDeclarator = new JassArrayDeclaratorSyntax(
                    arrayDeclarator.Type,
                    arrayDeclarator.ArrayToken,
                    renamedIdentifierName);

                return true;
            }

            renamedArrayDeclarator = null;
            return false;
        }
    }
}