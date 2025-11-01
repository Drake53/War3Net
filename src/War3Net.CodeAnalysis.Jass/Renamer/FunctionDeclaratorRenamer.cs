// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclaratorRenamer.cs" company="Drake53">
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
        private bool TryRenameFunctionDeclarator(JassFunctionDeclaratorSyntax functionDeclarator, [NotNullWhen(true)] out JassFunctionDeclaratorSyntax? renamedFunctionDeclarator)
        {
            if (TryRenameFunctionIdentifierName(functionDeclarator.IdentifierName, out var renamedIdentifierName))
            {
                renamedFunctionDeclarator = new JassFunctionDeclaratorSyntax(
                    functionDeclarator.ConstantToken,
                    functionDeclarator.FunctionToken,
                    renamedIdentifierName,
                    functionDeclarator.ParameterList,
                    functionDeclarator.ReturnClause);

                return true;
            }

            renamedFunctionDeclarator = null;
            return false;
        }
    }
}