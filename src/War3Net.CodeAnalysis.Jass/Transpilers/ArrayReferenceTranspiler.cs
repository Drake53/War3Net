// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Syntax.ArrayReferenceSyntax arrayReferenceNode)
        {
            _ = arrayReferenceNode ?? throw new ArgumentNullException(nameof(arrayReferenceNode));

            return SyntaxFactory.ElementAccessExpression(
                arrayReferenceNode.IdentifierNameNode.TranspileExpression())
                .AddArgumentListArguments(
                SyntaxFactory.Argument(
                    arrayReferenceNode.IndexExpressionNode.Transpile()));
        }

        public static ExpressionSyntax Transpile(this Syntax.ArrayReferenceSyntax arrayReferenceNode, out bool @const)
        {
            _ = arrayReferenceNode ?? throw new ArgumentNullException(nameof(arrayReferenceNode));

            @const = false;

            return SyntaxFactory.ElementAccessExpression(
                arrayReferenceNode.IdentifierNameNode.TranspileExpression())
                .AddArgumentListArguments(
                SyntaxFactory.Argument(
                    arrayReferenceNode.IndexExpressionNode.Transpile()));
        }
    }
}