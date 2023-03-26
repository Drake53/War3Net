// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassBinaryExpressionSyntax BinaryAdditionExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.PlusToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinarySubtractionExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.MinusToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryMultiplicationExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.AsteriskToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryDivisionExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.SlashToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryGreaterThanExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.GreaterThanToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryLessThanExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.LessThanToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryEqualsExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.EqualsEqualsToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryNotEqualsExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.ExclamationEqualsToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryGreaterOrEqualExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.GreaterThanEqualsToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryLessOrEqualExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.LessThanEqualsToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryAndExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.AndKeyword),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryOrExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.OrKeyword),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryExpression(JassExpressionSyntax left, JassSyntaxToken operatorToken, JassExpressionSyntax right)
        {
            var expressionKind = operatorToken.SyntaxKind switch
            {
                JassSyntaxKind.PlusToken => JassSyntaxKind.AddExpression,
                JassSyntaxKind.MinusToken => JassSyntaxKind.SubtractExpression,
                JassSyntaxKind.AsteriskToken => JassSyntaxKind.MultiplyExpression,
                JassSyntaxKind.SlashToken => JassSyntaxKind.DivideExpression,
                JassSyntaxKind.GreaterThanToken => JassSyntaxKind.GreaterThanExpression,
                JassSyntaxKind.LessThanToken => JassSyntaxKind.LessThanExpression,
                JassSyntaxKind.EqualsEqualsToken => JassSyntaxKind.EqualsExpression,
                JassSyntaxKind.ExclamationEqualsToken => JassSyntaxKind.NotEqualsExpression,
                JassSyntaxKind.GreaterThanEqualsToken => JassSyntaxKind.GreaterThanOrEqualExpression,
                JassSyntaxKind.LessThanEqualsToken => JassSyntaxKind.LessThanOrEqualExpression,
                JassSyntaxKind.AndKeyword => JassSyntaxKind.AndExpression,
                JassSyntaxKind.OrKeyword => JassSyntaxKind.OrExpression,

                _ => throw new ArgumentException("Invalid SyntaxKind.", nameof(operatorToken)),
            };

            return new JassBinaryExpressionSyntax(
                left,
                operatorToken,
                right);
        }
    }
}