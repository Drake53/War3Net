// ------------------------------------------------------------------------------
// <copyright file="FunctionCallTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Jass.Syntax.FunctionCallSyntax functionCallNode)
        {
            _ = functionCallNode ?? throw new ArgumentNullException(nameof(functionCallNode));

            var functionCall = SyntaxFactory.InvocationExpression(
                functionCallNode.IdentifierNameNode.TranspileExpression());

            if (functionCallNode.EmptyArgumentListNode is null)
            {
                return functionCall.AddArgumentListArguments(functionCallNode.ArgumentListNode.Transpile().ToArray());
            }

            return functionCall;
        }

        public static ExpressionSyntax Transpile(this Jass.Syntax.FunctionCallSyntax functionCallNode, out bool @const)
        {
            _ = functionCallNode ?? throw new ArgumentNullException(nameof(functionCallNode));

            @const = false;

            var functionCall = SyntaxFactory.InvocationExpression(
                functionCallNode.IdentifierNameNode.TranspileExpression());

            if (functionCallNode.EmptyArgumentListNode is null)
            {
                return functionCall.AddArgumentListArguments(functionCallNode.ArgumentListNode.Transpile().ToArray());
            }

            return functionCall;
        }
    }
}