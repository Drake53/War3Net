// ------------------------------------------------------------------------------
// <copyright file="GlobalVariableDeclarationRenamer.cs" company="Drake53">
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
        private bool TryRenameGlobalVariableDeclaration(JassGlobalVariableDeclarationSyntax globalVariableDeclaration, [NotNullWhen(true)] out JassGlobalDeclarationSyntax? renamedGlobalVariableDeclaration)
        {
            if (TryRenameVariableOrArrayDeclarator(globalVariableDeclaration.Declarator, out var renamedDeclarator))
            {
                renamedGlobalVariableDeclaration = new JassGlobalVariableDeclarationSyntax(renamedDeclarator);
                return true;
            }

            renamedGlobalVariableDeclaration = null;
            return false;
        }
    }
}