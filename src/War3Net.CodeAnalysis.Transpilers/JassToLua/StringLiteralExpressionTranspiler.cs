// ------------------------------------------------------------------------------
// <copyright file="StringLiteralExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassStringLiteralExpressionSyntax stringLiteralExpression, out JassTypeSyntax type)
        {
            type = JassTypeSyntax.String;

            return $"\"{stringLiteralExpression.Value.Replace($"{JassSymbol.CarriageReturn}", @"\r").Replace($"{JassSymbol.LineFeed}", @"\n")}\"";
        }
    }
}