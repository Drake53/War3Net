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
            Parser<char, JassSyntaxTrivia> optionalNewlineTriviaParser,
            Parser<char, JassSyntaxTrivia> singleLineCommentTriviaParser)
        {
            return OneOf(
                OneOf(
                    whitespaceTriviaParser,
                    singleLineCommentTriviaParser)
                    .Many()
                    .Then(optionalNewlineTriviaParser, (trivia, newline) => new JassSyntaxTriviaList(trivia.Append(newline).ToImmutableArray())),
                Return(JassSyntaxTriviaList.Empty));
        }
    }
}