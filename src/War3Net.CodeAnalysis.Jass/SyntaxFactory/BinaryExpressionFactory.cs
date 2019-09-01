// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionFactory.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax BinaryAdditionExpression(NewExpressionSyntax expr1, NewExpressionSyntax expr2)
        {
            return new NewExpressionSyntax(
                expr1.Expression,
                new BinaryExpressionTailSyntax(
                    new BinaryOperatorSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.PlusOperator), 0)),
                    expr2));
        }

        public static NewExpressionSyntax BinarySubtractionExpression(NewExpressionSyntax expr1, NewExpressionSyntax expr2)
        {
            return new NewExpressionSyntax(
                expr1.Expression,
                new BinaryExpressionTailSyntax(
                    new BinaryOperatorSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.MinusOperator), 0)),
                    expr2));
        }
    }
}