// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public string TranspileUnary(JassSyntaxKind unaryOperatorTokenSyntaxKind)
        {
            return unaryOperatorTokenSyntaxKind switch
            {
                JassSyntaxKind.PlusToken => LuaSyntaxNode.Tokens.Plus,
                JassSyntaxKind.MinusToken => LuaSyntaxNode.Tokens.Sub,
                JassSyntaxKind.NotKeyword => LuaSyntaxNode.Keyword.Not,
            };
        }
    }
}