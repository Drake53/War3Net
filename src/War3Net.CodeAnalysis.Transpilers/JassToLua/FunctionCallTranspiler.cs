// ------------------------------------------------------------------------------
// <copyright file="FunctionCallTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("functionCall")]
        public LuaExpressionSyntax? Transpile(FunctionCallSyntax? functionCall, out SyntaxTokenType expressionType)
        {
            if (functionCall is null)
            {
                expressionType = SyntaxTokenType.NullKeyword;
                return null;
            }

            expressionType = GetFunctionReturnType(functionCall.IdentifierNameNode.ValueText);

            var invocation = new LuaInvocationExpressionSyntax(TranspileExpression(functionCall.IdentifierNameNode));
            if (functionCall.ArgumentListNode is not null)
            {
                invocation.AddArguments(Transpile(functionCall.ArgumentListNode));
            }

            return invocation;
        }
    }
}