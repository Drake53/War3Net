// ------------------------------------------------------------------------------
// <copyright file="FunctionCallTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Syntax.FunctionCallSyntax functionCallNode)
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

        public static ExpressionSyntax Transpile(this Syntax.FunctionCallSyntax functionCallNode, out bool @const)
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