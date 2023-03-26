// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
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
                    type = JassTypeSyntax.Boolean;
                    break;

                default:
                    type = left.Equals(JassTypeSyntax.String) || right.Equals(JassTypeSyntax.String)
                        ? JassTypeSyntax.String
                        : left.Equals(JassTypeSyntax.Real) || right.Equals(JassTypeSyntax.Real)
                            ? JassTypeSyntax.Real
                            : left;
                    break;
            }

            return binaryOperatorTokenSyntaxKind switch
            {
                JassSyntaxKind.PlusToken => type.Equals(JassTypeSyntax.String) ? LuaSyntaxNode.Tokens.Concatenation : LuaSyntaxNode.Tokens.Plus,
                JassSyntaxKind.MinusToken => LuaSyntaxNode.Tokens.Sub,
                JassSyntaxKind.AsteriskToken => LuaSyntaxNode.Tokens.Multiply,
                JassSyntaxKind.SlashToken => type.Equals(JassTypeSyntax.Integer) ? LuaSyntaxNode.Tokens.IntegerDiv : LuaSyntaxNode.Tokens.Div,
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