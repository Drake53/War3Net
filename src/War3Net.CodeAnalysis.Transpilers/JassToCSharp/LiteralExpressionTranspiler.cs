// ------------------------------------------------------------------------------
// <copyright file="LiteralExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.Common.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassLiteralExpressionSyntax literalExpression)
        {
            return literalExpression.SyntaxKind switch
            {
                JassSyntaxKind.TrueLiteralExpression => TranspileLiteral(SyntaxKind.TrueLiteralExpression, SyntaxKind.TrueKeyword, literalExpression.Token),
                JassSyntaxKind.FalseLiteralExpression => TranspileLiteral(SyntaxKind.FalseLiteralExpression, SyntaxKind.FalseKeyword, literalExpression.Token),
                JassSyntaxKind.NullLiteralExpression => TranspileLiteral(SyntaxKind.NullLiteralExpression, SyntaxKind.NullKeyword, literalExpression.Token),
                JassSyntaxKind.DecimalLiteralExpression => TranspileDecimalLiteral(literalExpression),
                JassSyntaxKind.OctalLiteralExpression => TranspileOctalLiteral(literalExpression),
                JassSyntaxKind.HexadecimalLiteralExpression => TranspileHexadecimalLiteral(literalExpression),
                JassSyntaxKind.FourCCLiteralExpression => TranspileFourCCLiteral(literalExpression),
                JassSyntaxKind.CharacterLiteralExpression => TranspileCharacterLiteral(literalExpression),
                JassSyntaxKind.RealLiteralExpression => TranspileRealLiteral(literalExpression),
                JassSyntaxKind.StringLiteralExpression => TranspileStringLiteral(literalExpression),
            };
        }

        private ExpressionSyntax TranspileLiteral(SyntaxKind expressionKind, SyntaxKind tokenKind, JassSyntaxToken token)
        {
            return SyntaxFactory.LiteralExpression(
                expressionKind,
                Transpile(tokenKind, token));
        }

        private ExpressionSyntax TranspileDecimalLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var value = JassLiteral.ParseInt(literalExpression.Token.Text);
            var text = value.ToString(CultureInfo.InvariantCulture);

            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(
                    Transpile(literalExpression.Token.LeadingTrivia),
                    text,
                    value,
                    Transpile(literalExpression.Token.TrailingTrivia)));
        }

        private ExpressionSyntax TranspileOctalLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var value = JassLiteral.ParseOctal(literalExpression.Token.Text);
            var text = value.ToString(CultureInfo.InvariantCulture);

            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(
                    Transpile(literalExpression.Token.LeadingTrivia),
                    text,
                    value,
                    Transpile(literalExpression.Token.TrailingTrivia)));
        }

        private ExpressionSyntax TranspileHexadecimalLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var text = literalExpression.Token.Text.Replace(JassSymbol.Dollar, "0x", StringComparison.Ordinal);
            var value = Convert.ToInt32(text[2..], 16);

            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(
                    Transpile(literalExpression.Token.LeadingTrivia),
                    text,
                    value,
                    Transpile(literalExpression.Token.TrailingTrivia)));
        }

        private ExpressionSyntax TranspileFourCCLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var value = JassLiteral.ParseFourCC(literalExpression.Token.Text);
            var text = value.ToString(CultureInfo.InvariantCulture);

            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(
                    Transpile(literalExpression.Token.LeadingTrivia),
                    text,
                    value,
                    Transpile(literalExpression.Token.TrailingTrivia)));
        }

        private ExpressionSyntax TranspileCharacterLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var value = JassLiteral.ParseChar(literalExpression.Token.Text);
            var text = $"(int){literalExpression.Token.Text}";

            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(
                    Transpile(literalExpression.Token.LeadingTrivia),
                    text,
                    value,
                    Transpile(literalExpression.Token.TrailingTrivia)));
        }

        private ExpressionSyntax TranspileRealLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var text = literalExpression.Token.Text.TrimEnd(JassSymbol.DotChar);
            var value = float.Parse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(
                    Transpile(literalExpression.Token.LeadingTrivia),
                    $"{text}f",
                    value,
                    Transpile(literalExpression.Token.TrailingTrivia)));
        }

        private ExpressionSyntax TranspileStringLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var text = literalExpression.Token.Text
                .Replace(JassSymbol.CarriageReturn, @"\r", StringComparison.Ordinal)
                .Replace(JassSymbol.LineFeed, @"\n", StringComparison.Ordinal);

            return SyntaxFactory.LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Literal(
                    Transpile(literalExpression.Token.LeadingTrivia),
                    text,
                    text,
                    Transpile(literalExpression.Token.TrailingTrivia)));
        }
    }
}