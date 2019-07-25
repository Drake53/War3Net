// ------------------------------------------------------------------------------
// <copyright file="JassTokenizer.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.CodeAnalysis.Jass
{
    // TODO: handle escape character '\'
    internal class JassTokenizer : IDisposable
    {
        private const int BufferCapacity = 120;

        private readonly string _text;

        private TokenizerMode mode;

        public JassTokenizer(string text/*, JassParseOptions options*/)
        {
            _text = text;
        }

        public IEnumerable<SyntaxToken> Tokenize()
        {
            mode = TokenizerMode.Content;

            var buffer = new StringBuilder(BufferCapacity);

            using (var reader = new StringReader(_text))
            {
                while (true)
                {
                    var peek = reader.Peek();

                    // TODO: support multiCharacter delimiters (eg /* */ in vJASS)
                    if (IsCharacterDelimiter(peek))
                    {
                        if (buffer.Length == 0)
                        {
                            if (peek == -1)
                            {
                                break;
                            }

                            var peekChar = (char)peek;

                            if (SyntaxToken.TryTokenizeSingleSymbol(peekChar, out var token))
                            {
                                reader.Read();
                                peek = reader.Peek();

                                if (peek != -1 && SyntaxToken.TryTokenizeKeyword($"{peekChar}{(char)peek}", out var token2))
                                {
                                    reader.Read();

                                    // Assumption: non-alphanumeric tokens are at most 2 characters (==, !=, <=, >=, //).
                                    yield return TryUpdateMode(token2);
                                }
                                else
                                {
                                    yield return TryUpdateMode(token);
                                }

                                continue;
                            }
                            else if (char.IsWhiteSpace(peekChar))
                            {
                                reader.Read();
                                continue;
                            }
                        }
                        else
                        {
                            var tokenText = buffer.ToString();
                            buffer.Length = 0;

                            if (SyntaxToken.TryTokenizeKeyword(tokenText, out var token))
                            {
                                yield return TryUpdateMode(token);
                            }
                            else
                            {
                                yield return TryUpdateMode(new SyntaxToken(SyntaxToken.GetAlphanumericalTokenType(tokenText, mode), tokenText));
                            }

                            continue;
                        }
                    }

                    buffer.Append((char)reader.Read());
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private bool IsCharacterDelimiter(int c)
        {
            if (c == -1)
            {
                return true;
            }

            var character = (char)c;

            switch (mode)
            {
                // TODO: add constants for these characters
                case TokenizerMode.Content: return !char.IsLetterOrDigit(character) && character != '_' && character != '.' && character != '$';
                case TokenizerMode.String: return character == '"';
                case TokenizerMode.FourCC: return character == '\'';
                case TokenizerMode.SingleLineComment: return character == '\n';

                default: throw new InvalidOperationException();
            }
        }

        private SyntaxToken TryUpdateMode(SyntaxToken lastToken)
        {
            if (mode == TokenizerMode.SingleLineComment && lastToken.TokenType == SyntaxTokenType.NewlineSymbol)
            {
                mode = TokenizerMode.Content;
            }
            else if (mode == TokenizerMode.FourCC && lastToken.TokenType == SyntaxTokenType.SingleQuote)
            {
                mode = TokenizerMode.Content;
            }
            else if (mode == TokenizerMode.String && lastToken.TokenType == SyntaxTokenType.DoubleQuotes)
            {
                mode = TokenizerMode.Content;
            }
            else if (lastToken.TokenType == SyntaxTokenType.DoubleQuotes)
            {
                mode = TokenizerMode.String;
            }
            else if (lastToken.TokenType == SyntaxTokenType.SingleQuote)
            {
                mode = TokenizerMode.FourCC;
            }
            else if (lastToken.TokenType == SyntaxTokenType.DoubleForwardSlash)
            {
                mode = TokenizerMode.SingleLineComment;
            }

            return lastToken;
        }
    }
}