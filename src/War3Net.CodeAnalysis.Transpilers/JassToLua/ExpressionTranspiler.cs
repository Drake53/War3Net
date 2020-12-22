// ------------------------------------------------------------------------------
// <copyright file="ExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(ExpressionSyntax expression, out SyntaxTokenType expressionType)
        {
            _ = expression ?? throw new ArgumentNullException(nameof(expression));

            return Transpile(expression.UnaryExpression, out expressionType)
                ?? Transpile(expression.FunctionCall, out expressionType)
                ?? Transpile(expression.ArrayReference, out expressionType)
                ?? Transpile(expression.FunctionReference, out expressionType)
                ?? Transpile(expression.ConstantExpression, out expressionType)
                ?? Transpile(expression.ParenthesizedExpressionSyntax, out expressionType)
                ?? TranspileExpression(expression.Identifier, out expressionType)
                ?? throw new ArgumentNullException(nameof(expression));
        }
    }
}