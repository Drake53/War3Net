// ------------------------------------------------------------------------------
// <copyright file="CallStatementTranspiler.cs" company="Drake53">
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
        public static StatementSyntax Transpile(this Syntax.CallStatementSyntax callStatementNode)
        {
            _ = callStatementNode ?? throw new ArgumentNullException(nameof(callStatementNode));

            var invocation = SyntaxFactory.InvocationExpression(
                    callStatementNode.IdentifierNameNode.TranspileExpression());

            if (callStatementNode.EmptyArgumentListNode is null)
            {
                invocation = invocation.AddArgumentListArguments(
                    callStatementNode.ArgumentListNode.Transpile().ToArray());
            }

            return SyntaxFactory.ExpressionStatement(invocation);
        }
    }
}