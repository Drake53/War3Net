// ------------------------------------------------------------------------------
// <copyright file="ElseClauseTranspiler.cs" company="Drake53">
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
        public static ElseClauseSyntax Transpile(this Syntax.ElseClauseSyntax elseClauseNode)
        {
            _ = elseClauseNode ?? throw new ArgumentNullException(nameof(elseClauseNode));

            return SyntaxFactory.ElseClause(
                elseClauseNode.ElseifNode?.Transpile()
                ?? elseClauseNode.ElseNode.Transpile());
        }
    }
}