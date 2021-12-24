// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceExpressionRenamer.cs" company="Drake53">
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
        private bool TryRenameFunctionReferenceExpression(JassFunctionReferenceExpressionSyntax functionReferenceExpression, [NotNullWhen(true)] out IExpressionSyntax? renamedFunctionReferenceExpression)
        {
            if (TryRenameFunctionIdentifierName(functionReferenceExpression.IdentifierName, out var renamedIdentifierName))
            {
                renamedFunctionReferenceExpression = new JassFunctionReferenceExpressionSyntax(renamedIdentifierName);
                return true;
            }

            renamedFunctionReferenceExpression = null;
            return false;
        }
    }
}