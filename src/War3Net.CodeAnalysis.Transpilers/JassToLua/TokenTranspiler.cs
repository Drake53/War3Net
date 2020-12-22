// ------------------------------------------------------------------------------
// <copyright file="TokenTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        private const string AntiReservedKeywordConflictPrefix = "_";

        private static readonly Lazy<HashSet<string>> _reservedKeywords = new Lazy<HashSet<string>>(() => new HashSet<string>(GetReservedKeywords()));

        public LuaIdentifierNameSyntax TranspileIdentifier(TokenNode token)
        {
            if ((token?.TokenType ?? SyntaxTokenType.Undefined) == SyntaxTokenType.AlphanumericIdentifier)
            {
                return _reservedKeywords.Value.Contains(token.ValueText)
                    ? $"{AntiReservedKeywordConflictPrefix}{token.ValueText}"
                    : token.ValueText;
            }
            else
            {
                throw new ArgumentException($"Identifier token must have type {SyntaxTokenType.AlphanumericIdentifier}.");
            }
        }

        public string TranspileUnaryOperator(TokenNode token)
        {
            _ = token ?? throw new ArgumentNullException(nameof(token));

            return token.TokenType switch
            {
                SyntaxTokenType.PlusOperator => LuaSyntaxNode.Tokens.Plus,
                SyntaxTokenType.MinusOperator => LuaSyntaxNode.Tokens.Sub,
                SyntaxTokenType.NotOperator => LuaSyntaxNode.Keyword.Not,

                // todo: invalidenumargexc
                _ => throw new ArgumentException($"Cannot transpile token of type {token.TokenType} to an operator of a unary expression."),
            };
        }

        public string TranspileBinaryOperator(TokenNode token, SyntaxTokenType left, SyntaxTokenType right)
        {
            _ = token ?? throw new ArgumentNullException(nameof(token));

            return token.TokenType switch
            {
                SyntaxTokenType.PlusOperator => left == SyntaxTokenType.StringKeyword || right == SyntaxTokenType.StringKeyword ? LuaSyntaxNode.Tokens.Concatenation : LuaSyntaxNode.Tokens.Plus,
                SyntaxTokenType.MinusOperator => LuaSyntaxNode.Tokens.Sub,
                SyntaxTokenType.MultiplicationOperator => LuaSyntaxNode.Tokens.Multiply,
                SyntaxTokenType.DivisionOperator => left == SyntaxTokenType.IntegerKeyword && right == SyntaxTokenType.IntegerKeyword ? LuaSyntaxNode.Tokens.IntegerDiv : LuaSyntaxNode.Tokens.Div,
                SyntaxTokenType.GreaterThanOperator => ">",
                SyntaxTokenType.LessThanOperator => "<",
                SyntaxTokenType.EqualityOperator => LuaSyntaxNode.Tokens.EqualsEquals,
                SyntaxTokenType.UnequalityOperator => LuaSyntaxNode.Tokens.NotEquals,
                SyntaxTokenType.GreaterOrEqualOperator => ">=",
                SyntaxTokenType.LessOrEqualOperator => "<=",
                SyntaxTokenType.AndOperator => LuaSyntaxNode.Keyword.And,
                SyntaxTokenType.OrOperator => LuaSyntaxNode.Keyword.Or,

                // todo: invalidenumargexc
                _ => throw new ArgumentException($"Cannot transpile token of type {token.TokenType} to an operator of a binary expression."),
            };
        }

        [return: NotNullIfNotNull("token")]
        public LuaExpressionSyntax? TranspileExpression(TokenNode? token)
        {
            if (token is null)
            {
                return null;
            }

            if (token.TokenType == SyntaxTokenType.AlphanumericIdentifier)
            {
                return _reservedKeywords.Value.Contains(token.ValueText)
                    ? $"{AntiReservedKeywordConflictPrefix}{token.ValueText}"
                    : token.ValueText;
            }
            else
            {
                return GetConstantExpression(token);
            }
        }

        [return: NotNullIfNotNull("token")]
        public LuaExpressionSyntax? TranspileExpression(TokenNode? token, out SyntaxTokenType expressionType)
        {
            if (token is null)
            {
                expressionType = SyntaxTokenType.NullKeyword;
                return null;
            }

            expressionType = GetVariableType(token.ValueText);

            return TranspileExpression(token);
        }

        private string GetConstantExpression(TokenNode token)
        {
            var text = token.ValueText;
            switch (token.TokenType)
            {
                case SyntaxTokenType.DecimalNumber: return text;
                case SyntaxTokenType.OctalNumber: return "0";
                case SyntaxTokenType.HexadecimalNumber: return $"0x{text.Substring(text[0] == '$' ? 1 : 2)}";
                case SyntaxTokenType.FourCCNumber: return text.Length == 4
                    ? ((int)text[0] << 24 | (int)text[1] << 16 | (int)text[2] << 8 | (int)text[3]).ToString()
                    : ((int)text[0]).ToString();
                case SyntaxTokenType.RealNumber: return $"{(text[text.Length - 1] == '.' ? text.Substring(0, text.Length - 1) : text)}";
                case SyntaxTokenType.TrueKeyword: return "true";
                case SyntaxTokenType.FalseKeyword: return "false";
                case SyntaxTokenType.String: return $"\"{text.Replace("\r", @"\r").Replace("\n", @"\n")}\"";
                case SyntaxTokenType.NullKeyword: return "nil";

                default:
                    throw new ArgumentException($"Cannot transpile token of type {token.TokenType} to an expression.");
            }
        }

        private static IEnumerable<string> GetReservedKeywords()
        {
            yield return @"and";
            yield return @"break";
            yield return @"do";
            yield return @"else";
            yield return @"elseif";
            yield return @"end";
            yield return @"false";
            yield return @"for";
            yield return @"function";
            yield return @"if";
            yield return @"in";
            yield return @"local";
            yield return @"nil";
            yield return @"not";
            yield return @"or";
            yield return @"repeat";
            yield return @"return";
            yield return @"then";
            yield return @"true";
            yield return @"until";
            yield return @"while";
        }
    }
}