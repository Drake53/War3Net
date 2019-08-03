// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static Microsoft.CodeAnalysis.CSharp.SyntaxKind Transpile(this Syntax.UnaryOperatorSyntax unaryOperatorNode)
        {
            _ = unaryOperatorNode ?? throw new ArgumentNullException(nameof(unaryOperatorNode));

            return unaryOperatorNode.UnaryOperatorToken.TranspileUnaryOperator();
        }
    }
}