// ------------------------------------------------------------------------------
// <copyright file="CallStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this CallStatementSyntax callStatementNode, ref StringBuilder sb)
        {
            _ = callStatementNode ?? throw new ArgumentNullException(nameof(callStatementNode));

            callStatementNode.IdentifierNameNode.TranspileExpression(ref sb);
            sb.Append('(');
            if (callStatementNode.EmptyArgumentListNode is null)
            {
                callStatementNode.ArgumentListNode.Transpile(ref sb);
            }

            sb.Append(')');
        }

        public static LuaExpressionStatementSyntax TranspileToLua(this CallStatementSyntax callStatementNode)
        {
            _ = callStatementNode ?? throw new ArgumentNullException(nameof(callStatementNode));

            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                callStatementNode.IdentifierNameNode.TranspileExpressionToLua(),
                callStatementNode.ArgumentListNode?.TranspileToLua() ?? Array.Empty<LuaExpressionSyntax>()));
        }
    }
}