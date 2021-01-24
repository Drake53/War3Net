// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTranspiler.cs" company="Drake53">
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
        public string Transpile(UnaryOperatorType unaryOperator)
        {
            return unaryOperator switch
            {
                UnaryOperatorType.Plus => LuaSyntaxNode.Tokens.Plus,
                UnaryOperatorType.Minus => LuaSyntaxNode.Tokens.Sub,
                UnaryOperatorType.Not => LuaSyntaxNode.Keyword.Not,
            };
        }
    }
}