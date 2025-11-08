// ------------------------------------------------------------------------------
// <copyright file="LiteralExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassLiteralExpressionSyntax literalExpression, out JassTypeSyntax type)
        {
            return literalExpression.SyntaxKind switch
            {
                JassSyntaxKind.TrueLiteralExpression => TranspileTrueLiteral(out type),
                JassSyntaxKind.FalseLiteralExpression => TranspileFalseLiteral(out type),
                JassSyntaxKind.NullLiteralExpression => TranspileNullLiteral(out type),
                JassSyntaxKind.DecimalLiteralExpression => TranspileDecimalLiteral(literalExpression, out type),
                JassSyntaxKind.OctalLiteralExpression => TranspileOctalLiteral(literalExpression, out type),
                JassSyntaxKind.HexadecimalLiteralExpression => TranspileHexadecimalLiteral(literalExpression, out type),
                JassSyntaxKind.FourCCLiteralExpression => TranspileFourCCLiteral(literalExpression, out type),
                JassSyntaxKind.CharacterLiteralExpression => TranspileCharacterLiteral(literalExpression, out type),
                JassSyntaxKind.RealLiteralExpression => TranspileRealLiteral(literalExpression, out type),
                JassSyntaxKind.StringLiteralExpression => TranspileStringLiteral(literalExpression, out type),
            };
        }

        private LuaExpressionSyntax TranspileTrueLiteral(out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Boolean;
            return new LuaIdentifierLiteralExpressionSyntax(LuaIdentifierNameSyntax.True);
        }

        private LuaExpressionSyntax TranspileFalseLiteral(out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Boolean;
            return new LuaIdentifierLiteralExpressionSyntax(LuaIdentifierNameSyntax.False);
        }

        private LuaExpressionSyntax TranspileNullLiteral(out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Handle;
            return new LuaIdentifierLiteralExpressionSyntax(LuaIdentifierNameSyntax.Nil);
        }

        private LuaExpressionSyntax TranspileDecimalLiteral(JassLiteralExpressionSyntax literalExpression, out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Integer;
            return new LuaIdentifierLiteralExpressionSyntax(literalExpression.Token.Text);
        }

        private LuaExpressionSyntax TranspileOctalLiteral(JassLiteralExpressionSyntax literalExpression, out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Integer;
            var text = Convert.ToInt32(literalExpression.Token.Text, 8).ToString(CultureInfo.InvariantCulture);

            return new LuaIdentifierLiteralExpressionSyntax(text);
        }

        private LuaExpressionSyntax TranspileHexadecimalLiteral(JassLiteralExpressionSyntax literalExpression, out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Integer;
            var text = literalExpression.Token.Text.Replace(JassSymbol.Dollar, "0x", StringComparison.Ordinal);

            return new LuaIdentifierLiteralExpressionSyntax(text);
        }

        private LuaExpressionSyntax TranspileFourCCLiteral(JassLiteralExpressionSyntax literalExpression, out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Integer;
            var text = $"FourCC(\"{literalExpression.Token.Text.Trim(JassSymbol.SingleQuoteChar)}\")";

            return new LuaIdentifierLiteralExpressionSyntax(text);
        }

        private LuaExpressionSyntax TranspileCharacterLiteral(JassLiteralExpressionSyntax literalExpression, out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Integer;
            var text = ((int)char.Parse(literalExpression.Token.Text.Trim(JassSymbol.SingleQuoteChar))).ToString(CultureInfo.InvariantCulture);

            return new LuaIdentifierLiteralExpressionSyntax(text);
        }

        private LuaExpressionSyntax TranspileRealLiteral(JassLiteralExpressionSyntax literalExpression, out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Real;
            return new LuaIdentifierLiteralExpressionSyntax(literalExpression.Token.Text);
        }

        private LuaExpressionSyntax TranspileStringLiteral(JassLiteralExpressionSyntax literalExpression, out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.String;
            var text = literalExpression.Token.Text
                .Replace(JassSymbol.CarriageReturn, @"\r", StringComparison.Ordinal)
                .Replace(JassSymbol.LineFeed, @"\n", StringComparison.Ordinal);

            return new LuaStringLiteralExpressionSyntax(text);
        }
    }
}