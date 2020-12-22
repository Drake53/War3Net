// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementTranspiler.cs" company="Drake53">
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
        [return: NotNullIfNotNull("returnStatement")]
        public LuaStatementSyntax? Transpile(ReturnStatementSyntax? returnStatement)
        {
            if (returnStatement is null)
            {
                return null;
            }

            return returnStatement.ExpressionNode is null
                ? new LuaReturnStatementSyntax()
                : new LuaReturnStatementSyntax(Transpile(returnStatement.ExpressionNode));
        }
    }
}