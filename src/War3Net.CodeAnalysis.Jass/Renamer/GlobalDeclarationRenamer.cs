// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationRenamer.cs" company="Drake53">
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
        private bool TryRenameGlobalDeclaration(JassGlobalDeclarationSyntax globalDeclaration, [NotNullWhen(true)] out IGlobalDeclarationSyntax? renamedGlobalDeclaration)
        {
            if (TryRenameVariableDeclarator(globalDeclaration.Declarator, out var renamedDeclarator))
            {
                renamedGlobalDeclaration = new JassGlobalDeclarationSyntax(renamedDeclarator);
                return true;
            }

            renamedGlobalDeclaration = null;
            return false;
        }

        private bool TryRenameGlobalDeclaration(IGlobalDeclarationSyntax declaration, [NotNullWhen(true)] out IGlobalDeclarationSyntax? renamedDeclaration)
        {
            return declaration switch
            {
                JassGlobalDeclarationSyntax globalDeclaration => TryRenameGlobalDeclaration(globalDeclaration, out renamedDeclaration),

                _ => TryRenameDummy(declaration, out renamedDeclaration),
            };
        }
    }
}