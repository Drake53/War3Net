namespace War3Net.CodeAnalysis.Jass.Parsing
{
    internal sealed class JassLexer
    {
        private readonly string _source;
        private readonly DiagnosticBag _diagnostics;
        private readonly string? _filePath;
        private int _position;

        internal JassLexer(string source, DiagnosticBag diagnostics, string? filePath = null)
        {
            _source = source;
            _diagnostics = diagnostics;
            _filePath = filePath;
        }

        internal LexerResult Lex()
        {
            var estimatedTokenCount = _source.Length / 5;
            var tokens = ImmutableArray.CreateBuilder<JassSyntaxToken>(estimatedTokenCount);
            var offsets = ImmutableArray.CreateBuilder<int>(estimatedTokenCount);
            var leadingTriviaBuilder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>();
            var trailingTriviaBuilder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>();

            ScanTrivia(leadingTriviaBuilder, isTrailing: false);

            while (_position < _source.Length)
            {
                var tokenStart = _position;
                var tokenLeadingTrivia = ToTriviaList(leadingTriviaBuilder);
                leadingTriviaBuilder.Clear();

                var (kind, text) = ScanToken();

                trailingTriviaBuilder.Clear();
                ScanTrivia(trailingTriviaBuilder, isTrailing: true);

                var token = new JassSyntaxToken(
                    tokenLeadingTrivia,
                    kind,
                    text,
                    ToTriviaList(trailingTriviaBuilder));

                tokens.Add(token);
                offsets.Add(tokenStart);

                ScanTrivia(leadingTriviaBuilder, isTrailing: false);
            }

            var eofToken = new JassSyntaxToken(
                ToTriviaList(leadingTriviaBuilder),
                JassSyntaxKind.EndOfFileToken,
                string.Empty,
                JassSyntaxTriviaList.Empty);

            tokens.Add(eofToken);
            offsets.Add(_position);

            tokens.Capacity = tokens.Count;
            offsets.Capacity = offsets.Count;
            return new LexerResult(tokens.MoveToImmutable(), offsets.MoveToImmutable());
        }

        private static JassSyntaxTriviaList ToTriviaList(ImmutableArray<JassSyntaxTrivia>.Builder builder)
        {
            if (builder.Count == 0)
            {
                return JassSyntaxTriviaList.Empty;
            }

            if (builder.Count == 1)
            {
                var trivia = builder[0];
                if (ReferenceEquals(trivia, JassSyntaxTrivia.SingleSpace))
                {
                    return JassSyntaxTriviaList.SingleSpace;
                }

                if (ReferenceEquals(trivia, JassSyntaxTrivia.NewLine))
                {
                    return JassSyntaxTriviaList.NewLine;
                }

                if (ReferenceEquals(trivia, JassSyntaxTrivia.LineFeed))
                {
                    return JassSyntaxTriviaList.LineFeed;
                }

                if (ReferenceEquals(trivia, JassSyntaxTrivia.CarriageReturn))
                {
                    return JassSyntaxTriviaList.CarriageReturn;
                }
            }

            return new JassSyntaxTriviaList(builder.ToImmutable());
        }

        private (JassSyntaxKind Kind, string Text) ScanToken()
        {
            var ch = _source[_position];

            if (JassSyntaxFacts.IsIdentifierStartCharacter(ch))
            {
                return ScanIdentifierOrKeyword();
            }

            if (ch >= '0' && ch <= '9')
            {
                return ScanNumericLiteral();
            }

            switch (ch)
            {
                case JassSymbol.DollarChar:
                    return ScanHexLiteral();

                case JassSymbol.SingleQuoteChar:
                    return ScanCharOrFourCCLiteral();

                case JassSymbol.DoubleQuoteChar:
                    return ScanStringLiteral();

                case JassSymbol.DotChar:
                    if (_position + 1 < _source.Length && _source[_position + 1] >= '0' && _source[_position + 1] <= '9')
                    {
                        return ScanRealLiteralFromDot();
                    }

                    _diagnostics.Report(
                        JassSyntaxDiagnostics.InvalidNumber,
                        CreateLocation(_position, 1),
                        JassSymbol.Dot);

                    _position++;
                    return (JassSyntaxKind.RealLiteralToken, JassSymbol.Dot);

                case JassSymbol.OpenParenChar:
                    _position++;
                    return (JassSyntaxKind.OpenParenToken, JassSymbol.OpenParen);

                case JassSymbol.CloseParenChar:
                    _position++;
                    return (JassSyntaxKind.CloseParenToken, JassSymbol.CloseParen);

                case JassSymbol.OpenBracketChar:
                    _position++;
                    return (JassSyntaxKind.OpenBracketToken, JassSymbol.OpenBracket);

                case JassSymbol.CloseBracketChar:
                    _position++;
                    return (JassSyntaxKind.CloseBracketToken, JassSymbol.CloseBracket);

                case JassSymbol.CommaChar:
                    _position++;
                    return (JassSyntaxKind.CommaToken, JassSymbol.Comma);

                case JassSymbol.PlusChar:
                    _position++;
                    return (JassSyntaxKind.PlusToken, JassSymbol.Plus);

                case JassSymbol.MinusChar:
                    _position++;
                    return (JassSyntaxKind.MinusToken, JassSymbol.Minus);

                case JassSymbol.AsteriskChar:
                    _position++;
                    return (JassSyntaxKind.AsteriskToken, JassSymbol.Asterisk);

                case JassSymbol.SlashChar:
                    _position++;
                    return (JassSyntaxKind.SlashToken, JassSymbol.Slash);

                case JassSymbol.EqualsChar:
                    if (_position + 1 < _source.Length && _source[_position + 1] == JassSymbol.EqualsChar)
                    {
                        _position += 2;
                        return (JassSyntaxKind.EqualsEqualsToken, JassSymbol.EqualsEquals);
                    }

                    _position++;
                    return (JassSyntaxKind.EqualsToken, JassSymbol.Equals);

                case JassSymbol.ExclamationChar:
                    if (_position + 1 < _source.Length && _source[_position + 1] == JassSymbol.EqualsChar)
                    {
                        _position += 2;
                        return (JassSyntaxKind.ExclamationEqualsToken, JassSymbol.ExclamationEquals);
                    }

                    _diagnostics.Report(
                        JassSyntaxDiagnostics.InvalidCharacter,
                        CreateLocation(_position, 1),
                        "!");

                    _position++;
                    return (JassSyntaxKind.None, "!");

                case JassSymbol.LessThanChar:
                    if (_position + 1 < _source.Length && _source[_position + 1] == JassSymbol.EqualsChar)
                    {
                        _position += 2;
                        return (JassSyntaxKind.LessThanEqualsToken, JassSymbol.LessThanEquals);
                    }

                    _position++;
                    return (JassSyntaxKind.LessThanToken, JassSymbol.LessThan);

                case JassSymbol.GreaterThanChar:
                    if (_position + 1 < _source.Length && _source[_position + 1] == JassSymbol.EqualsChar)
                    {
                        _position += 2;
                        return (JassSyntaxKind.GreaterThanEqualsToken, JassSymbol.GreaterThanEquals);
                    }

                    _position++;
                    return (JassSyntaxKind.GreaterThanToken, JassSymbol.GreaterThan);

                default:
                    _diagnostics.Report(
                        JassSyntaxDiagnostics.InvalidCharacter,
                        CreateLocation(_position, 1),
                        ch.ToString());

                    _position++;
                    return (JassSyntaxKind.None, ch.ToString());
            }
        }

        private (JassSyntaxKind Kind, string Text) ScanIdentifierOrKeyword()
        {
            var start = _position;
            _position++;

            while (_position < _source.Length && JassSyntaxFacts.IsIdentifierPartCharacter(_source[_position]))
            {
                _position++;
            }

            var span = _source.AsSpan(start, _position - start);
            var kind = JassSyntaxFacts.GetSyntaxKind(span);

            if (kind != JassSyntaxKind.None)
            {
                return (kind, JassSyntaxFacts.GetText(kind));
            }

            return (JassSyntaxKind.IdentifierToken, _source[start.._position]);
        }

        private (JassSyntaxKind Kind, string Text) ScanNumericLiteral()
        {
            var start = _position;

            if (_source[_position] == JassSymbol.ZeroChar && _position + 1 < _source.Length)
            {
                var next = _source[_position + 1];
                if (next == JassSymbol.XChar || next == JassSymbol.XCharUpper)
                {
                    _position += 2;
                    while (_position < _source.Length && JassSyntaxFacts.IsHexDigit(_source[_position]))
                    {
                        _position++;
                    }

                    var text = _source[start.._position];
                    if (_position - start == 2)
                    {
                        _diagnostics.Report(
                            JassSyntaxDiagnostics.InvalidNumber,
                            CreateLocation(start, 2),
                            text);
                    }

                    return (JassSyntaxKind.HexadecimalLiteralToken, text);
                }

                if (next >= '0' && next <= '9')
                {
                    _position += 2;
                    var hasNonOctalDigit = next >= '8';

                    while (_position < _source.Length && _source[_position] >= '0' && _source[_position] <= '9')
                    {
                        hasNonOctalDigit |= _source[_position] >= '8';
                        _position++;
                    }

                    if (_position < _source.Length && _source[_position] == JassSymbol.DotChar)
                    {
                        _position++;
                        while (_position < _source.Length && _source[_position] >= '0' && _source[_position] <= '9')
                        {
                            _position++;
                        }

                        return (JassSyntaxKind.RealLiteralToken, _source[start.._position]);
                    }

                    var octalText = _source[start.._position];
                    if (hasNonOctalDigit)
                    {
                        _diagnostics.Report(
                            JassSyntaxDiagnostics.InvalidNumber,
                            CreateLocation(start, _position - start),
                            octalText);
                    }

                    return (JassSyntaxKind.OctalLiteralToken, octalText);
                }
            }

            while (_position < _source.Length && _source[_position] >= '0' && _source[_position] <= '9')
            {
                _position++;
            }

            if (_position < _source.Length && _source[_position] == JassSymbol.DotChar)
            {
                _position++;
                while (_position < _source.Length && _source[_position] >= '0' && _source[_position] <= '9')
                {
                    _position++;
                }

                return (JassSyntaxKind.RealLiteralToken, _source[start.._position]);
            }

            return (JassSyntaxKind.DecimalLiteralToken, _source[start.._position]);
        }

        private (JassSyntaxKind Kind, string Text) ScanHexLiteral()
        {
            var start = _position;
            _position++; // skip '$'

            while (_position < _source.Length && JassSyntaxFacts.IsHexDigit(_source[_position]))
            {
                _position++;
            }

            var text = _source[start.._position];
            if (_position - start == 1)
            {
                _diagnostics.Report(
                    JassSyntaxDiagnostics.InvalidNumber,
                    CreateLocation(start, 1),
                    text);
            }

            return (JassSyntaxKind.HexadecimalLiteralToken, text);
        }

        private (JassSyntaxKind Kind, string Text) ScanRealLiteralFromDot()
        {
            var start = _position;
            _position++; // skip '.'

            while (_position < _source.Length && _source[_position] >= '0' && _source[_position] <= '9')
            {
                _position++;
            }

            return (JassSyntaxKind.RealLiteralToken, _source[start.._position]);
        }

        private (JassSyntaxKind Kind, string Text) ScanCharOrFourCCLiteral()
        {
            var start = _position;
            _position++; // skip opening quote

            var charCount = 0;
            while (_position < _source.Length && _source[_position] != JassSymbol.SingleQuoteChar)
            {
                if (_source[_position] == '\\' && _position + 1 < _source.Length)
                {
                    var escapeStart = _position;
                    _position++;
                    if (!JassSyntaxFacts.IsValidEscapeCharacter(_source[_position]))
                    {
                        _diagnostics.Report(
                            JassSyntaxDiagnostics.InvalidEscapeSequence,
                            CreateLocation(escapeStart, 2),
                            $"\\{_source[_position]}");
                    }
                }

                _position++;
                charCount++;
            }

            var unterminated = false;
            if (_position < _source.Length && _source[_position] == JassSymbol.SingleQuoteChar)
            {
                _position++;
            }
            else
            {
                unterminated = true;
                _diagnostics.Report(
                    JassSyntaxDiagnostics.UnterminatedSingleQuotedLiteral,
                    CreateLocation(start, _position - start));
            }

            var text = _source[start.._position];

            if (charCount == 4)
            {
                return (JassSyntaxKind.FourCCLiteralToken, text);
            }

            if (!unterminated && charCount != 1)
            {
                _diagnostics.Report(
                    JassSyntaxDiagnostics.InvalidSingleQuotedStringLength,
                    CreateLocation(start, _position - start),
                    text);
            }

            return (JassSyntaxKind.CharacterLiteralToken, text);
        }

        private (JassSyntaxKind Kind, string Text) ScanStringLiteral()
        {
            var start = _position;
            _position++; // skip opening quote

            while (_position < _source.Length && _source[_position] != JassSymbol.DoubleQuoteChar)
            {
                if (_source[_position] == '\\')
                {
                    var escapeStart = _position;
                    _position++;
                    if (_position < _source.Length)
                    {
                        if (!JassSyntaxFacts.IsValidEscapeCharacter(_source[_position]))
                        {
                            _diagnostics.Report(
                                JassSyntaxDiagnostics.InvalidEscapeSequence,
                                CreateLocation(escapeStart, 2),
                                $"\\{_source[_position]}");
                        }

                        _position++;
                    }
                }
                else
                {
                    _position++;
                }
            }

            if (_position < _source.Length)
            {
                _position++; // skip closing quote
            }
            else
            {
                _diagnostics.Report(
                    JassSyntaxDiagnostics.UnterminatedString,
                    CreateLocation(start, _position - start));
            }

            return (JassSyntaxKind.StringLiteralToken, _source[start.._position]);
        }

        private void ScanTrivia(ImmutableArray<JassSyntaxTrivia>.Builder builder, bool isTrailing)
        {
            while (_position < _source.Length)
            {
                var ch = _source[_position];

                if (JassSyntaxFacts.IsWhitespaceCharacter(ch))
                {
                    ScanWhitespace(builder);
                    continue;
                }

                if (JassSyntaxFacts.IsNewLineCharacter(ch))
                {
                    ScanNewLine(builder);

                    if (isTrailing)
                    {
                        return;
                    }

                    continue;
                }

                if (ch == JassSymbol.SlashChar && _position + 1 < _source.Length && _source[_position + 1] == JassSymbol.SlashChar)
                {
                    ScanSingleLineComment(builder);
                    continue;
                }

                break;
            }
        }

        private void ScanWhitespace(ImmutableArray<JassSyntaxTrivia>.Builder builder)
        {
            var start = _position;

            while (_position < _source.Length && JassSyntaxFacts.IsWhitespaceCharacter(_source[_position]))
            {
                _position++;
            }

            if (_position - start == 1 && _source[start] == JassSymbol.SpaceChar)
            {
                builder.Add(JassSyntaxTrivia.SingleSpace);
            }
            else
            {
                builder.Add(new JassSyntaxTrivia(JassSyntaxKind.WhitespaceTrivia, _source[start.._position]));
            }
        }

        private void ScanNewLine(ImmutableArray<JassSyntaxTrivia>.Builder builder)
        {
            if (_source[_position] == JassSymbol.CarriageReturnChar)
            {
                _position++;
                if (_position < _source.Length && _source[_position] == JassSymbol.LineFeedChar)
                {
                    _position++;
                    builder.Add(JassSyntaxTrivia.NewLine);
                }
                else
                {
                    builder.Add(JassSyntaxTrivia.CarriageReturn);
                }
            }
            else
            {
                _position++;
                builder.Add(JassSyntaxTrivia.LineFeed);
            }
        }

        private void ScanSingleLineComment(ImmutableArray<JassSyntaxTrivia>.Builder builder)
        {
            var start = _position;
            _position += 2; // skip //

            while (_position < _source.Length && !JassSyntaxFacts.IsNewLineCharacter(_source[_position]))
            {
                _position++;
            }

            builder.Add(new JassSyntaxTrivia(JassSyntaxKind.SingleLineCommentTrivia, _source[start.._position]));
        }

        private Location CreateLocation(int start, int length)
        {
            var span = new TextSpan(start, length);
            return Location.Create(span, _filePath);
        }

        internal readonly struct LexerResult
        {
            internal LexerResult(ImmutableArray<JassSyntaxToken> tokens, ImmutableArray<int> tokenOffsets)
            {
                Tokens = tokens;
                TokenOffsets = tokenOffsets;
            }

            internal ImmutableArray<JassSyntaxToken> Tokens { get; }

            internal ImmutableArray<int> TokenOffsets { get; }
        }
    }
}