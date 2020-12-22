// ------------------------------------------------------------------------------
// <copyright file="StringTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("string")]
        public LuaExpressionSyntax? Transpile(StringSyntax? @string)
        {
            if (@string is null)
            {
                return null;
            }

            return @string.StringNode is null ? LuaStringLiteralExpressionSyntax.Empty : TranspileExpression(@string.StringNode);
        }
    }
}