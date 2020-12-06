// ------------------------------------------------------------------------------
// <copyright file="FunctionCallTranspiler.cs" company="Drake53">
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
        public static void Transpile(this FunctionCallSyntax functionCallNode, ref StringBuilder sb, out bool isString)
        {
            _ = functionCallNode ?? throw new ArgumentNullException(nameof(functionCallNode));

            isString = TranspileStringConcatenationHandler.IsFunctionStringReturnType(functionCallNode.IdentifierNameNode.ValueText);

            functionCallNode.IdentifierNameNode.TranspileExpression(ref sb);
            sb.Append('(');
            if (functionCallNode.EmptyArgumentListNode is null)
            {
                functionCallNode.ArgumentListNode.Transpile(ref sb);
            }

            sb.Append(')');
        }

        public static LuaExpressionSyntax TranspileToLua(this FunctionCallSyntax functionCallNode, out bool isString)
        {
            _ = functionCallNode ?? throw new ArgumentNullException(nameof(functionCallNode));

            isString = TranspileStringConcatenationHandler.IsFunctionStringReturnType(functionCallNode.IdentifierNameNode.ValueText);

            var invocation = new LuaInvocationExpressionSyntax(functionCallNode.IdentifierNameNode.TranspileExpressionToLua());
            if (functionCallNode.ArgumentListNode is not null)
            {
                invocation.AddArguments(functionCallNode.ArgumentListNode.TranspileToLua());
            }

            return invocation;
        }
    }
}