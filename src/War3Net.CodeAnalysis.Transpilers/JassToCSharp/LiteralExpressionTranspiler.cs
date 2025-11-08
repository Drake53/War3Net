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
                JassSyntaxKind.TrueLiteralExpression => SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression),
                JassSyntaxKind.FalseLiteralExpression => SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression),
                JassSyntaxKind.NullLiteralExpression => SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression),
                JassSyntaxKind.DecimalLiteralExpression => SyntaxFactory.ParseExpression(literalExpression.Token.Text),
                JassSyntaxKind.OctalLiteralExpression => TranspileOctalLiteral(literalExpression),
                JassSyntaxKind.HexadecimalLiteralExpression => TranspileHexadecimalLiteral(literalExpression),
                JassSyntaxKind.FourCCLiteralExpression => TranspileFourCCLiteral(literalExpression),
                JassSyntaxKind.CharacterLiteralExpression => TranspileCharacterLiteral(literalExpression),
                JassSyntaxKind.RealLiteralExpression => TranspileRealLiteral(literalExpression),
                JassSyntaxKind.StringLiteralExpression => TranspileStringLiteral(literalExpression),
            };
        }

        private ExpressionSyntax TranspileOctalLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var text = Convert.ToInt32(literalExpression.Token.Text, 8).ToString(CultureInfo.InvariantCulture);
            return SyntaxFactory.ParseExpression(text);
        }

        private ExpressionSyntax TranspileHexadecimalLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var text = literalExpression.Token.Text.Replace(JassSymbol.Dollar, "0x", StringComparison.Ordinal);
            return SyntaxFactory.ParseExpression(text);
        }

        private ExpressionSyntax TranspileFourCCLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var text = literalExpression.Token.Text.Trim(JassSymbol.SingleQuoteChar).FromRawcode().ToString(CultureInfo.InvariantCulture);
            return SyntaxFactory.ParseExpression(text);
        }

        private ExpressionSyntax TranspileCharacterLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var text = ((int)char.Parse(literalExpression.Token.Text.Trim(JassSymbol.SingleQuoteChar))).ToString(CultureInfo.InvariantCulture);
            return SyntaxFactory.ParseExpression(text);
        }

        private ExpressionSyntax TranspileRealLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var text = $"{literalExpression.Token.Text.TrimEnd(JassSymbol.DotChar)}f";
            return SyntaxFactory.ParseExpression(text);
        }

        private ExpressionSyntax TranspileStringLiteral(JassLiteralExpressionSyntax literalExpression)
        {
            var text = literalExpression.Token.Text
                .Replace(JassSymbol.CarriageReturn, @"\r", StringComparison.Ordinal)
                .Replace(JassSymbol.LineFeed, @"\n", StringComparison.Ordinal);

            return SyntaxFactory.ParseExpression(text);
        }
    }
}