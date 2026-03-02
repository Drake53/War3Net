using System.Collections.Immutable;
using System.Linq;
using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassSyntaxTriviaList> GetSimpleTriviaListParser(
            Parser<char, JassSyntaxTrivia> whitespaceTriviaParser)
        {
            return OneOf(
                whitespaceTriviaParser.Many().Select(trivia => new JassSyntaxTriviaList(trivia.ToImmutableArray())),
                Return(JassSyntaxTriviaList.Empty));
        }

        internal static Parser<char, JassSyntaxTriviaList> GetLeadingTriviaListParser(
            Parser<char, JassSyntaxTrivia> whitespaceTriviaParser,
            Parser<char, JassSyntaxTrivia> newLineTriviaParser,
            Parser<char, JassSyntaxTrivia> singleLineCommentTriviaParser)
        {
            return OneOf(
                OneOf(
                    whitespaceTriviaParser,
                    newLineTriviaParser,
                    singleLineCommentTriviaParser)
                    .Many()
                    .Select(trivia => new JassSyntaxTriviaList(trivia.ToImmutableArray())),
                Return(JassSyntaxTriviaList.Empty));
        }

        internal static Parser<char, JassSyntaxTriviaList> GetTrailingTriviaListParser(
            Parser<char, JassSyntaxTrivia> whitespaceTriviaParser,
            Parser<char, JassSyntaxTrivia> singleNewLineTriviaParser,
            Parser<char, JassSyntaxTrivia> singleLineCommentTriviaParser)
        {
            return OneOf(
                OneOf(
                    whitespaceTriviaParser,
                    singleLineCommentTriviaParser)
                    .Many()
                    .Then(
                        OneOf(
                            singleNewLineTriviaParser.Select(newLine => Maybe.Just(newLine)),
                            End.ThenReturn(Maybe.Nothing<JassSyntaxTrivia>())),
                        (trivia, newLine) => new JassSyntaxTriviaList((newLine.HasValue ? trivia.Append(newLine.Value) : trivia).ToImmutableArray())),
                Return(JassSyntaxTriviaList.Empty));
        }
    }
}