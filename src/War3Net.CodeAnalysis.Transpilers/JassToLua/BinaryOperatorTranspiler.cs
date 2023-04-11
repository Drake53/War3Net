// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public string TranspileBinary(JassSyntaxKind binaryOperatorTokenSyntaxKind, JassTypeSyntax left, JassTypeSyntax right, out JassTypeSyntax type)
        {
            switch (binaryOperatorTokenSyntaxKind)
            {
                case JassSyntaxKind.GreaterThanToken:
                case JassSyntaxKind.LessThanToken:
                case JassSyntaxKind.EqualsToken:
                case JassSyntaxKind.ExclamationEqualsToken:
                case JassSyntaxKind.GreaterThanEqualsToken:
                case JassSyntaxKind.LessThanEqualsToken:
                case JassSyntaxKind.AndKeyword:
                case JassSyntaxKind.OrKeyword:
                    type = JassPredefinedTypeSyntax.Boolean;
                    break;

                default:
                    var leftKind = left.GetToken().SyntaxKind;
                    var rightKind = right.GetToken().SyntaxKind;
                    type = leftKind == JassSyntaxKind.StringKeyword || rightKind == JassSyntaxKind.StringKeyword
                        ? JassPredefinedTypeSyntax.String
                        : leftKind == JassSyntaxKind.RealKeyword || rightKind == JassSyntaxKind.RealKeyword
                            ? JassPredefinedTypeSyntax.Real
                            : left;
                    break;
            }

            var kind = type.GetToken().SyntaxKind;
            return binaryOperatorTokenSyntaxKind switch
            {
                JassSyntaxKind.PlusToken => kind == JassSyntaxKind.StringKeyword ? LuaSyntaxNode.Tokens.Concatenation : LuaSyntaxNode.Tokens.Plus,
                JassSyntaxKind.MinusToken => LuaSyntaxNode.Tokens.Sub,
                JassSyntaxKind.AsteriskToken => LuaSyntaxNode.Tokens.Multiply,
                JassSyntaxKind.SlashToken => kind == JassSyntaxKind.IntegerKeyword ? LuaSyntaxNode.Tokens.IntegerDiv : LuaSyntaxNode.Tokens.Div,
                JassSyntaxKind.GreaterThanToken => ">",
                JassSyntaxKind.LessThanToken => "<",
                JassSyntaxKind.EqualsToken => LuaSyntaxNode.Tokens.EqualsEquals,
                JassSyntaxKind.ExclamationEqualsToken => LuaSyntaxNode.Tokens.NotEquals,
                JassSyntaxKind.GreaterThanEqualsToken => ">=",
                JassSyntaxKind.LessThanEqualsToken => "<=",
                JassSyntaxKind.AndKeyword => LuaSyntaxNode.Keyword.And,
                JassSyntaxKind.OrKeyword => LuaSyntaxNode.Keyword.Or,
            };
        }
    }
}