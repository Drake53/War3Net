using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileParenthesizedExpression(
            JassParenthesizedExpressionSyntax parenthesizedExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return TryDecompileTriggerFunctionParameter(parenthesizedExpression.Expression, expectedType, out functionParameter);
        }

        private bool TryDecompileParenthesizedExpression(
            JassParenthesizedExpressionSyntax parenthesizedExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            return TryDecompileTriggerFunctionParameter(parenthesizedExpression.Expression, out decompileOptions);
        }
    }
}