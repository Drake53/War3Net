// ------------------------------------------------------------------------------
// <copyright file="SyntaxTriviaListParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
            Parser<char, JassSyntaxTrivia> newlineTriviaParser,
            Parser<char, JassSyntaxTrivia> singleLineCommentTriviaParser)
        {
            return OneOf(
                OneOf(
                    whitespaceTriviaParser,
                    newlineTriviaParser,
                    singleLineCommentTriviaParser)
                    .Many()
                    .Select(trivia => new JassSyntaxTriviaList(trivia.ToImmutableArray())),
                Return(JassSyntaxTriviaList.Empty));
        }

        internal static Parser<char, JassSyntaxTriviaList> GetTrailingTriviaListParser(
            Parser<char, JassSyntaxTrivia> whitespaceTriviaParser,
            Parser<char, JassSyntaxTrivia> singleNewlineTriviaParser,
            Parser<char, JassSyntaxTrivia> singleLineCommentTriviaParser)
        {
            return OneOf(
                OneOf(
                    whitespaceTriviaParser,
                    singleLineCommentTriviaParser)
                    .Many()
                    .Then(
                        OneOf(
                            singleNewlineTriviaParser.Select(newline => Maybe.Just(newline)),
                            End.ThenReturn(Maybe.Nothing<JassSyntaxTrivia>())),
                        (trivia, newline) => new JassSyntaxTriviaList((newline.HasValue ? trivia.Append(newline.Value) : trivia).ToImmutableArray())),
                Return(JassSyntaxTriviaList.Empty));
        }
    }
}