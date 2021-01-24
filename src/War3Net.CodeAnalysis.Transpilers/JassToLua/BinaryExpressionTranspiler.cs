// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassBinaryExpressionSyntax binaryExpression, out JassTypeSyntax type)
        {
            var left = Transpile(binaryExpression.Left, out var leftType);
            var right = Transpile(binaryExpression.Right, out var rightType);

            return new LuaBinaryExpressionSyntax(left, Transpile(binaryExpression.Operator, leftType, rightType, out type), right);
        }
    }
}