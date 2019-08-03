// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Syntax.FunctionReferenceSyntax functionReferenceNode)
        {
            _ = functionReferenceNode ?? throw new ArgumentNullException(nameof(functionReferenceNode));

            return functionReferenceNode.IdentifierNameNode.TranspileExpression();
            // return SyntaxFactory.ParseExpression(functionReferenceNode.IdentifierNameNode.TranspileType());
        }

        public static ExpressionSyntax Transpile(this Syntax.FunctionReferenceSyntax functionReferenceNode, out bool @const)
        {
            _ = functionReferenceNode ?? throw new ArgumentNullException(nameof(functionReferenceNode));

            @const = false;

            return functionReferenceNode.IdentifierNameNode.TranspileExpression();
            // return SyntaxFactory.ParseExpression(functionReferenceNode.IdentifierNameNode.TranspileType());
        }
    }
}