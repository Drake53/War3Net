// ------------------------------------------------------------------------------
// <copyright file="SyntaxToken.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass
{
    struct SyntaxToken
    {
        // TODO: add constants for all keywords
        public const string TypeKeyword = "type";
        public const string NewlineSymbol = "\n";

        public readonly SyntaxTokenType TokenType;
        public readonly string TokenValue;

        private static readonly Lazy<Dictionary<SyntaxTokenType, string>> _defaultTokenValues = new Lazy<Dictionary<SyntaxTokenType, string>>(() => InitializeDefaultTokenValues());
        private static readonly Lazy<Dictionary<char, SyntaxTokenType>> _singleSymbolTokens = new Lazy<Dictionary<char, SyntaxTokenType>>(() => InitializeSingleSymbolTokens());
        private static readonly Lazy<Dictionary<string, SyntaxTokenType>> _keywordTokens = new Lazy<Dictionary<string, SyntaxTokenType>>(() => InitializeKeywordTokens());

        public SyntaxToken(SyntaxTokenType tokenType)
        {
            TokenType = tokenType;
            TokenValue = GetDefaultTokenValue(tokenType) ?? throw new ArgumentException($"{tokenType} has no default value.", nameof(tokenType));
        }

        public SyntaxToken(SyntaxTokenType tokenType, string tokenValue)
        {
            TokenType = tokenType;
            TokenValue = tokenValue;
        }

        public override string ToString()
        {
            return TokenValue == GetDefaultTokenValue(TokenType)
                ? $"[{TokenType}]"
                : $"[{TokenType}] {new string(TokenValue.Where(c => c != '\r').ToArray())}";
        }

        public static bool TryTokenizeSingleSymbol(char symbol, out SyntaxToken token)
        {
            if (_singleSymbolTokens.Value.TryGetValue(symbol, out var tokenType))
            {
                token = new SyntaxToken(tokenType, symbol.ToString());
                return true;
            }

            token = default;
            return false;
        }

        public static bool TryTokenizeKeyword(string keyword, out SyntaxToken token)
        {
            if (_keywordTokens.Value.TryGetValue(keyword, out var tokenType))
            {
                token = new SyntaxToken(tokenType, keyword);
                return true;
            }

            token = default;
            return false;
        }

        public static string GetDefaultTokenValue(SyntaxTokenType tokenType)
        {
            return _defaultTokenValues.Value.TryGetValue(tokenType, out var value) ? value : null;
        }

        public static SyntaxTokenType GetAlphanumericalTokenType(string token, TokenizerMode mode)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));

            switch (mode)
            {
                case TokenizerMode.String: return SyntaxTokenType.String;
                case TokenizerMode.FourCC: return SyntaxTokenType.FourCCNumber;
                case TokenizerMode.SingleLineComment: return SyntaxTokenType.Comment;
                default:
                    {
                        var firstChar = token[0];

                        if (firstChar == '$') return SyntaxTokenType.HexadecimalNumber;
                        if (token.Contains('.')) return SyntaxTokenType.RealNumber;
                        if (!char.IsDigit(firstChar)) return SyntaxTokenType.AlphanumericIdentifier;

                        if (firstChar == '0')
                        {
                            if ((token.Length >= 2 ? char.ToLower(token[1]) : '\0') == 'x') return SyntaxTokenType.HexadecimalNumber;
                            return SyntaxTokenType.OctalNumber;
                        }

                        return SyntaxTokenType.DecimalNumber;
                    }
            }
        }

        private static Dictionary<SyntaxTokenType, string> InitializeDefaultTokenValues()
        {
            var result = new Dictionary<SyntaxTokenType, string>();
            result.Add(SyntaxTokenType.Undefined, null);

            // Keywords
            result.Add(SyntaxTokenType.TypeKeyword, TypeKeyword);
            result.Add(SyntaxTokenType.ExtendsKeyword, "extends");
            result.Add(SyntaxTokenType.HandleKeyword, "handle");
            result.Add(SyntaxTokenType.GlobalsKeyword, "globals");
            result.Add(SyntaxTokenType.EndglobalsKeyword, "endglobals");
            result.Add(SyntaxTokenType.ConstantKeyword, "constant");
            result.Add(SyntaxTokenType.NativeKeyword, "native");
            result.Add(SyntaxTokenType.TakesKeyword, "takes");
            result.Add(SyntaxTokenType.NothingKeyword, "nothing");
            result.Add(SyntaxTokenType.ReturnsKeyword, "returns");
            result.Add(SyntaxTokenType.FunctionKeyword, "function");
            result.Add(SyntaxTokenType.EndfunctionKeyword, "endfunction");
            result.Add(SyntaxTokenType.LocalKeyword, "local");
            result.Add(SyntaxTokenType.ArrayKeyword, "array");
            result.Add(SyntaxTokenType.SetKeyword, "set");
            result.Add(SyntaxTokenType.CallKeyword, "call");
            result.Add(SyntaxTokenType.IfKeyword, "if");
            result.Add(SyntaxTokenType.ThenKeyword, "then");
            result.Add(SyntaxTokenType.EndifKeyword, "endif");
            result.Add(SyntaxTokenType.ElseKeyword, "else");
            result.Add(SyntaxTokenType.ElseifKeyword, "elseif");
            result.Add(SyntaxTokenType.LoopKeyword, "loop");
            result.Add(SyntaxTokenType.EndloopKeyword, "endloop");
            result.Add(SyntaxTokenType.ExitwhenKeyword, "exitwhen");
            result.Add(SyntaxTokenType.ReturnKeyword, "return");
            result.Add(SyntaxTokenType.DebugKeyword, "debug");
            result.Add(SyntaxTokenType.NullKeyword, "null");
            result.Add(SyntaxTokenType.TrueKeyword, "true");
            result.Add(SyntaxTokenType.FalseKeyword, "false");
            result.Add(SyntaxTokenType.CodeKeyword, "code");
            result.Add(SyntaxTokenType.IntegerKeyword, "integer");
            result.Add(SyntaxTokenType.RealKeyword, "real");
            result.Add(SyntaxTokenType.BooleanKeyword, "boolean");
            result.Add(SyntaxTokenType.StringKeyword, "string");

            // Operators
            result.Add(SyntaxTokenType.PlusOperator, "+");
            result.Add(SyntaxTokenType.MinusOperator, "-");
            result.Add(SyntaxTokenType.MultiplicationOperator, "*");
            result.Add(SyntaxTokenType.DivisionOperator, "/");
            result.Add(SyntaxTokenType.GreaterThanOperator, ">");
            result.Add(SyntaxTokenType.LessThanOperator, "<");
            result.Add(SyntaxTokenType.EqualityOperator, "==");
            result.Add(SyntaxTokenType.UnequalityOperator, "!=");
            result.Add(SyntaxTokenType.GreaterOrEqualOperator, ">=");
            result.Add(SyntaxTokenType.LessOrEqualOperator, "<=");
            result.Add(SyntaxTokenType.AndOperator, "and");
            result.Add(SyntaxTokenType.OrOperator, "or");
            result.Add(SyntaxTokenType.NotOperator, "not");

            // Other Symbols
            result.Add(SyntaxTokenType.ParenthesisOpenSymbol, "(");
            result.Add(SyntaxTokenType.ParenthesisCloseSymbol, ")");
            result.Add(SyntaxTokenType.SquareBracketOpenSymbol, "[");
            result.Add(SyntaxTokenType.SquareBracketCloseSymbol, "]");
            result.Add(SyntaxTokenType.Assignment, "=");
            result.Add(SyntaxTokenType.NewlineSymbol, NewlineSymbol);
            result.Add(SyntaxTokenType.SingleQuote, "'");
            result.Add(SyntaxTokenType.DoubleQuotes, "\"");
            result.Add(SyntaxTokenType.Comma, ",");
            result.Add(SyntaxTokenType.DoubleForwardSlash, "//");

            // Alphanumericals
            result.Add(SyntaxTokenType.AlphanumericIdentifier, null);
            result.Add(SyntaxTokenType.DecimalNumber, null);
            result.Add(SyntaxTokenType.OctalNumber, null);
            result.Add(SyntaxTokenType.HexadecimalNumber, null);
            result.Add(SyntaxTokenType.FourCCNumber, null);
            result.Add(SyntaxTokenType.RealNumber, null);
            result.Add(SyntaxTokenType.String, null);
            result.Add(SyntaxTokenType.Comment, null);

            return result;
        }

        private static Dictionary<char, SyntaxTokenType> InitializeSingleSymbolTokens()
        {
            var result = new Dictionary<char, SyntaxTokenType>();

            foreach (var pair in _defaultTokenValues.Value)
            {
                if ((pair.Value?.Length ?? 0) == 1)
                {
                    result.Add(pair.Value[0], pair.Key);
                }
            }

            return result;
        }

        private static Dictionary<string, SyntaxTokenType> InitializeKeywordTokens()
        {
            var result = new Dictionary<string, SyntaxTokenType>();

            foreach (var pair in _defaultTokenValues.Value)
            {
                if ((pair.Value?.Length ?? 0) > 1)
                {
                    result.Add(pair.Value, pair.Key);
                }
            }

            return result;
        }
    }
}