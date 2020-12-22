// ------------------------------------------------------------------------------
// <copyright file="CallStatementTranspiler.cs" company="Drake53">
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
        [return: NotNullIfNotNull("callStatement")]
        public LuaStatementSyntax? Transpile(CallStatementSyntax? callStatement)
        {
            if (callStatement is null)
            {
                return null;
            }

            var expression = TranspileExpression(callStatement.IdentifierNameNode);
            var invocation = callStatement.ArgumentListNode is null
                ? new LuaInvocationExpressionSyntax(expression)
                : new LuaInvocationExpressionSyntax(expression, Transpile(callStatement.ArgumentListNode));

            return new LuaExpressionStatementSyntax(invocation);
        }
    }
}