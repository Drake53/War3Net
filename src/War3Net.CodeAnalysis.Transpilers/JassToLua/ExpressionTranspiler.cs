// ------------------------------------------------------------------------------
// <copyright file="ExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this ExpressionSyntax expressionNode, ref StringBuilder sb, out bool isString)
        {
            _ = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));

            isString = false;
            if (expressionNode.Identifier != null)
            {
                isString = TranspileStringConcatenationHandler.IsStringVariable(expressionNode.Identifier.ValueText);
            }

            expressionNode.UnaryExpression?.Transpile(ref sb);
            expressionNode.FunctionCall?.Transpile(ref sb, out isString);
            expressionNode.ArrayReference?.Transpile(ref sb, out isString);
            expressionNode.FunctionReference?.Transpile(ref sb);
            expressionNode.ConstantExpression?.Transpile(ref sb, out isString);
            expressionNode.ParenthesizedExpressionSyntax?.Transpile(ref sb, out isString);
            expressionNode.Identifier?.TranspileExpression(ref sb);
        }
    }
}