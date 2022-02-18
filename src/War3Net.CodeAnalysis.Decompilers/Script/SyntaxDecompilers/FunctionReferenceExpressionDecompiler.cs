// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceExpressionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileFunctionReferenceExpression(
            JassFunctionReferenceExpressionSyntax functionReferenceExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            functionParameter = null;
            return false;
        }

        private bool TryDecompileFunctionReferenceExpression(
            JassFunctionReferenceExpressionSyntax functionReferenceExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            decompileOptions = null;
            return false;
        }
    }
}