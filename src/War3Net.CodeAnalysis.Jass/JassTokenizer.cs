// ------------------------------------------------------------------------------
// <copyright file="JassTokenizer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.CodeAnalysis.Jass
{
    [Obsolete]
    internal class JassTokenizer : IDisposable
    {
        private const int BufferCapacity = 120;

        private readonly string _text;

        private TokenizerMode _mode;

        public JassTokenizer(string text/*, JassParseOptions options*/)
        {
            _text = text;
        }

        public IEnumerable<SyntaxToken> Tokenize()
        {
            _mode = TokenizerMode.Content;

            var buffer = new StringBuilder(BufferCapacity);

            const char EscapeCharacter = '\\';
            var encounteredEscapeCharacter = false;

            using (var reader = new StringReader(_text))
            {
                while (true)
                {
                    var peek = reader.Peek();

                    if (peek == EscapeCharacter)
                    {
                        if (_mode == TokenizerMode.Content)
                        {
                            throw new Exception($"Unexpectedly encountered escape character '{EscapeCharacter}'");
                        }

                        if (!encounteredEscapeCharacter)
                        {
                            encounteredEscapeCharacter = true;
                            buffer.Append((char)reader.Read());
                            continue;
                        }
                    }

                    var ignoreDelimiter = encounteredEscapeCharacter;
                    encounteredEscapeCharacter = false;

                    // TODO: support multiCharacter delimiters (eg /* */ in vJASS)
                    if (!ignoreDelimiter && IsCharacterDelimiter(peek))
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
                            else
                            {
                                // Handle 2-char symbols for which the first symbol on its own is not a token.
                                reader.Read();
                                peek = reader.Read();

                                if (SyntaxToken.TryTokenizeKeyword($"{peekChar}{(char)peek}", out var token2))
                                {
                                    yield return TryUpdateMode(token2);
                                    continue;
                                }

                                throw new InvalidDataException($"Invalid sequence of symbols: {peekChar}{(char)peek}");
                            }
                        }
                        else
                        {
                            var tokenText = buffer.ToString();
                            buffer.Length = 0;

                            if (_mode == TokenizerMode.Content && SyntaxToken.TryTokenizeKeyword(tokenText, out var token))
                            {
                                yield return TryUpdateMode(token);
                            }
                            else
                            {
                                yield return TryUpdateMode(new SyntaxToken(SyntaxToken.GetAlphanumericalTokenType(tokenText, _mode), tokenText));
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

            switch (_mode)
            {
                // TODO: add constants for these characters
                case TokenizerMode.Content: return !char.IsLetterOrDigit(character) && character != '_' && character != '.' && character != '$';
                case TokenizerMode.String: return character == '"';
                case TokenizerMode.FourCC: return character == '\'';
                case TokenizerMode.SingleLineComment: return character == '\n' || character == '\r';

                default: throw new InvalidOperationException();
            }
        }

        private SyntaxToken TryUpdateMode(SyntaxToken lastToken)
        {
            if (_mode == TokenizerMode.SingleLineComment && lastToken.TokenType == SyntaxTokenType.NewlineSymbol)
            {
                _mode = TokenizerMode.Content;
            }
            else if (_mode == TokenizerMode.FourCC && lastToken.TokenType == SyntaxTokenType.SingleQuote)
            {
                _mode = TokenizerMode.Content;
            }
            else if (_mode == TokenizerMode.String && lastToken.TokenType == SyntaxTokenType.DoubleQuotes)
            {
                _mode = TokenizerMode.Content;
            }
            else if (lastToken.TokenType == SyntaxTokenType.DoubleQuotes)
            {
                _mode = TokenizerMode.String;
            }
            else if (lastToken.TokenType == SyntaxTokenType.SingleQuote)
            {
                _mode = TokenizerMode.FourCC;
            }
            else if (lastToken.TokenType == SyntaxTokenType.DoubleForwardSlash)
            {
                _mode = TokenizerMode.SingleLineComment;
            }

            return lastToken;
        }
    }
}