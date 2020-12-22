// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("unaryExpression")]
        public LuaExpressionSyntax? Transpile(UnaryExpressionSyntax? unaryExpression, out SyntaxTokenType expressionType)
        {
            if (unaryExpression is null)
            {
                expressionType = SyntaxTokenType.NullKeyword;
                return null;
            }

            return new LuaPrefixUnaryExpressionSyntax(
                Transpile(unaryExpression.ExpressionNode, out expressionType),
                Transpile(unaryExpression.UnaryOperatorNode));
        }
    }
}