// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationRenamer.cs" company="Drake53">
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
        private bool TryRenameFunctionDeclaration(JassFunctionDeclarationSyntax functionDeclaration, [NotNullWhen(true)] out ITopLevelDeclarationSyntax? renamedFunctionDeclaration)
        {
            foreach (var parameter in functionDeclaration.FunctionDeclarator.ParameterList.Parameters)
            {
                _localVariableNames.Add(parameter.IdentifierName.Name);
            }

            if (TryRenameFunctionDeclarator(functionDeclaration.FunctionDeclarator, out var renamedFunctionDeclarator) |
                TryRenameStatementList(functionDeclaration.Body, out var renamedBody))
            {
                _localVariableNames.Clear();

                renamedFunctionDeclaration = new JassFunctionDeclarationSyntax(
                    renamedFunctionDeclarator ?? functionDeclaration.FunctionDeclarator,
                    renamedBody ?? functionDeclaration.Body);

                return true;
            }

            _localVariableNames.Clear();

            renamedFunctionDeclaration = null;
            return false;
        }
    }
}