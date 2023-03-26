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
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassLiteralExpressionSyntax literalExpression, out JassTypeSyntax type)
        {
            switch (literalExpression.Token.SyntaxKind)
            {
                case JassSyntaxKind.TrueKeyword:
                    type = JassTypeSyntax.Boolean;
                    return LuaIdentifierLiteralExpressionSyntax.True;

                case JassSyntaxKind.FalseKeyword:
                    type = JassTypeSyntax.Boolean;
                    return LuaIdentifierLiteralExpressionSyntax.False;

                case JassSyntaxKind.NullKeyword:
                    type = JassTypeSyntax.Handle;
                    return LuaIdentifierLiteralExpressionSyntax.Nil;

                case JassSyntaxKind.CharacterLiteralToken:
                    type = JassTypeSyntax.Integer;
                    return new LuaCharacterLiteralExpression(literalExpression.Token.Text[0]);

                case JassSyntaxKind.DecimalLiteralToken:
                    type = JassTypeSyntax.Integer;
                    return int.Parse(literalExpression.Token.Text, CultureInfo.InvariantCulture);

                case JassSyntaxKind.FourCCLiteralToken:
                    type = JassTypeSyntax.Integer;
                    return literalExpression.Token.Text.FromJassRawcode();

                case JassSyntaxKind.HexadecimalLiteralToken:
                    type = JassTypeSyntax.Integer;
                    return literalExpression.Token.Text.Replace(JassSymbol.Dollar, "0x", false, CultureInfo.InvariantCulture);

                case JassSyntaxKind.OctalLiteralToken:
                    type = JassTypeSyntax.Integer;
                    return Convert.ToInt32(literalExpression.Token.Text, 8);

                case JassSyntaxKind.RealLiteralToken:
                    type = JassTypeSyntax.Real;
                    return literalExpression.Token.Text.TrimEnd(JassSymbol.DotChar);

                case JassSyntaxKind.StringLiteralToken:
                    type = JassTypeSyntax.String;
                    return literalExpression.Token.Text
                        .Replace(JassSymbol.CarriageReturn, @"\r", false, CultureInfo.InvariantCulture)
                        .Replace(JassSymbol.LineFeed, @"\n", false, CultureInfo.InvariantCulture);

                default: throw new NotSupportedException();
            }
        }
    }
}