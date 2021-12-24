// ------------------------------------------------------------------------------
// <copyright file="IdentifierNameRenamer.cs" company="Drake53">
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
        private bool TryRenameFunctionIdentifierName(JassIdentifierNameSyntax identifierName, [NotNullWhen(true)] out JassIdentifierNameSyntax? renamedIdentifierName)
        {
            return _functionDeclarationRenames.TryGetValue(identifierName.Name, out renamedIdentifierName);
        }

        private bool TryRenameVariableIdentifierName(JassIdentifierNameSyntax identifierName, [NotNullWhen(true)] out JassIdentifierNameSyntax? renamedIdentifierName)
        {
            if (_localVariableNames.Contains(identifierName.Name))
            {
                renamedIdentifierName = null;
                return false;
            }

            return _globalVariableRenames.TryGetValue(identifierName.Name, out renamedIdentifierName);
        }
    }
}