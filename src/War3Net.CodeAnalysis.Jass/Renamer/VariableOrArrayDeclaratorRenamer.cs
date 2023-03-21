// ------------------------------------------------------------------------------
// <copyright file="VariableOrArrayDeclaratorRenamer.cs" company="Drake53">
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
        private bool TryRenameVariableOrArrayDeclarator(JassVariableOrArrayDeclaratorSyntax declarator, [NotNullWhen(true)] out JassVariableOrArrayDeclaratorSyntax? renamedDeclarator)
        {
            return declarator switch
            {
                JassArrayDeclaratorSyntax arrayDeclarator => TryRenameArrayDeclarator(arrayDeclarator, out renamedDeclarator),
                JassVariableDeclaratorSyntax variableDeclarator => TryRenameVariableDeclarator(variableDeclarator, out renamedDeclarator),

                _ => TryRenameDummy(declarator, out renamedDeclarator),
            };
        }
    }
}