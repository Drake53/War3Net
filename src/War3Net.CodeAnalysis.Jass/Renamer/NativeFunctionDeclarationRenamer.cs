// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationRenamer.cs" company="Drake53">
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
        private bool TryRenameNativeFunctionDeclaration(JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration, [NotNullWhen(true)] out ITopLevelDeclarationSyntax? renamedNativeFunctionDeclaration)
        {
            if (TryRenameFunctionDeclarator(nativeFunctionDeclaration.FunctionDeclarator, out var renamedFunctionDeclarator))
            {
                renamedNativeFunctionDeclaration = new JassNativeFunctionDeclarationSyntax(renamedFunctionDeclarator);
                return true;
            }

            renamedNativeFunctionDeclaration = null;
            return false;
        }
    }
}