// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionFactory.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax UnaryPlusExpression(NewExpressionSyntax expression)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new UnaryExpressionSyntax(
                        new UnaryOperatorSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.PlusOperator), 0)),
                        expression)),
                new EmptyNode(0));
        }

        public static NewExpressionSyntax UnaryMinusExpression(NewExpressionSyntax expression)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new UnaryExpressionSyntax(
                        new UnaryOperatorSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.MinusOperator), 0)),
                        expression)),
                new EmptyNode(0));
        }
    }
}