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
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassLiteralExpressionSyntax literalExpression)
        {
            switch (literalExpression.Token.SyntaxKind)
            {
                case JassSyntaxKind.TrueKeyword:
                    return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);

                case JassSyntaxKind.FalseKeyword:
                    return SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);

                case JassSyntaxKind.NullKeyword:
                    return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);

                case JassSyntaxKind.CharacterLiteralToken:
                    return SyntaxFactory.ParseExpression($"((int){literalExpression.Token.Text})");

                case JassSyntaxKind.DecimalLiteralToken:
                    return SyntaxFactory.ParseExpression(literalExpression.Token.Text);

                case JassSyntaxKind.FourCCLiteralToken:
                    return SyntaxFactory.ParseExpression(literalExpression.Token.Text.FromJassRawcode().ToString(CultureInfo.InvariantCulture));

                case JassSyntaxKind.HexadecimalLiteralToken:
                    return SyntaxFactory.ParseExpression(literalExpression.Token.Text.Replace(JassSymbol.Dollar, "0x", false, CultureInfo.InvariantCulture));

                case JassSyntaxKind.OctalLiteralToken:
                    return SyntaxFactory.ParseExpression(Convert.ToInt32(literalExpression.Token.Text, 8).ToString(CultureInfo.InvariantCulture));

                case JassSyntaxKind.RealLiteralToken:
                    return SyntaxFactory.ParseExpression($"{literalExpression.Token.Text.TrimEnd(JassSymbol.DotChar)}f");

                case JassSyntaxKind.StringLiteralToken:
                    return SyntaxFactory.ParseExpression(literalExpression.Token.Text
                        .Replace(JassSymbol.CarriageReturn, @"\r", false, CultureInfo.InvariantCulture)
                        .Replace(JassSymbol.LineFeed, @"\n", false, CultureInfo.InvariantCulture));

                default: throw new NotSupportedException();
            }
        }
    }
}