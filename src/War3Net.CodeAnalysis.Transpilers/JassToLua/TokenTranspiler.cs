// ------------------------------------------------------------------------------
// <copyright file="TokenTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        private const string AntiReservedKeywordConflictPrefix = "_";

        private static readonly Lazy<HashSet<string>> _reservedKeywords = new Lazy<HashSet<string>>(() => new HashSet<string>(GetReservedKeywords()));

        public static void TranspileIdentifier(this TokenNode tokenNode, ref StringBuilder sb)
        {
            if ((tokenNode?.TokenType ?? SyntaxTokenType.Undefined) == SyntaxTokenType.AlphanumericIdentifier)
            {
                sb.Append(_reservedKeywords.Value.Contains(tokenNode.ValueText)
                    ? $"{AntiReservedKeywordConflictPrefix}{tokenNode.ValueText}"
                    : tokenNode.ValueText);
            }
            else
            {
                throw new ArgumentException($"Identifier token must have type {SyntaxTokenType.AlphanumericIdentifier}.");
            }
        }

        public static void TranspileUnaryOperator(this TokenNode tokenNode, ref StringBuilder sb)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            switch (tokenNode.TokenType)
            {
                case SyntaxTokenType.PlusOperator: sb.Append('+'); break;
                case SyntaxTokenType.MinusOperator: sb.Append('-'); break;
                case SyntaxTokenType.NotOperator: sb.Append("not "); break;

                default:
                    throw new ArgumentException($"Cannot transpile token of type {tokenNode.TokenType} to an operator of a unary expression.");
            }
        }

        public static void TranspileBinaryOperator(this TokenNode tokenNode, bool isString, ref StringBuilder sb)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            switch (tokenNode.TokenType)
            {
                case SyntaxTokenType.PlusOperator: sb.Append(isString ? ".." : "+"); break;
                case SyntaxTokenType.MinusOperator: sb.Append('-'); break;
                case SyntaxTokenType.MultiplicationOperator: sb.Append('*'); break;
                case SyntaxTokenType.DivisionOperator: sb.Append('/'); break;
                case SyntaxTokenType.GreaterThanOperator: sb.Append('>'); break;
                case SyntaxTokenType.LessThanOperator: sb.Append('<'); break;
                case SyntaxTokenType.EqualityOperator: sb.Append("=="); break;
                case SyntaxTokenType.UnequalityOperator: sb.Append("~="); break;
                case SyntaxTokenType.GreaterOrEqualOperator: sb.Append(">="); break;
                case SyntaxTokenType.LessOrEqualOperator: sb.Append("<="); break;
                case SyntaxTokenType.AndOperator: sb.Append("and"); break;
                case SyntaxTokenType.OrOperator: sb.Append("or"); break;

                default:
                    throw new ArgumentException($"Cannot transpile token of type {tokenNode.TokenType} to an operator of a binary expression.");
            }
        }

        public static void TranspileExpression(this TokenNode tokenNode, ref StringBuilder sb)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            if (tokenNode.TokenType == SyntaxTokenType.AlphanumericIdentifier)
            {
                sb.Append(_reservedKeywords.Value.Contains(tokenNode.ValueText)
                    ? $"{AntiReservedKeywordConflictPrefix}{tokenNode.ValueText}"
                    : tokenNode.ValueText);
            }
            else
            {
                sb.Append(tokenNode.GetConstantExpression());
            }
        }

        private static string GetConstantExpression(this TokenNode tokenNode)
        {
            var text = tokenNode.ValueText;
            switch (tokenNode.TokenType)
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
                case SyntaxTokenType.String: return $"\"{text}\"";
                case SyntaxTokenType.NullKeyword: return "nil";

                default:
                    throw new ArgumentException($"Cannot transpile token of type {tokenNode.TokenType} to an expression.");
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