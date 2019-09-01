// ------------------------------------------------------------------------------
// <copyright file="ArgumentListFactory.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static ArgumentListSyntax ArgumentList(params NewExpressionSyntax[] expressions)
        {
            if (expressions.Length == 0)
            {
                throw new System.Exception();
            }

            if (expressions.Length == 1)
            {
                return new ArgumentListSyntax(expressions[0], new ArgumentListTailSyntax(new EmptyNode(0)));
            }

            return new ArgumentListSyntax(
                expressions[0],
                new ArgumentListTailSyntax(
                    expressions.Skip(1).Select(
                        expr => new CommaSeparatedExpressionSyntax(
                            new TokenNode(new SyntaxToken(SyntaxTokenType.Comma), 0),
                            expr)).ToArray()));
        }
    }
}