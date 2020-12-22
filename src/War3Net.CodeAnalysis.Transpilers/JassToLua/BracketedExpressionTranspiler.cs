// ------------------------------------------------------------------------------
// <copyright file="BracketedExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(BracketedExpressionSyntax bracketedExpression)
        {
            _ = bracketedExpression ?? throw new ArgumentNullException(nameof(bracketedExpression));

            return Transpile(bracketedExpression.ExpressionNode);
        }
    }
}