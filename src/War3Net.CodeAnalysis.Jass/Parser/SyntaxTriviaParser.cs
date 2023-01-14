// ------------------------------------------------------------------------------
// <copyright file="SyntaxTriviaParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassSyntaxTrivia> GetWhitespaceTriviaParser()
        {
            return Token(JassSyntaxFacts.IsWhitespaceCharacter)
                .AtLeastOnceString()
                .Select(whitespace => new JassSyntaxTrivia(JassSyntaxKind.WhitespaceTrivia, whitespace));
        }

        internal static Parser<char, JassSyntaxTrivia> GetNewlineTriviaParser()
        {
            return OneOf(Symbol.CarriageReturn, Symbol.LineFeed)
                .AtLeastOnceString()
                .Select(newline => new JassSyntaxTrivia(JassSyntaxKind.NewlineTrivia, newline));
        }

        internal static Parser<char, JassSyntaxTrivia> GetOptionalNewlineTriviaParser()
        {
            return OneOf(Symbol.CarriageReturn, Symbol.LineFeed)
                .ManyString()
                .Select(newline => new JassSyntaxTrivia(JassSyntaxKind.NewlineTrivia, newline));
        }

        internal static Parser<char, JassSyntaxTrivia> GetSingleLineCommentTriviaParser()
        {
            return Map(
                (_, commentString) => new JassSyntaxTrivia(JassSyntaxKind.SingleLineCommentTrivia, $"{JassSymbol.SlashSlash}{commentString}"),
                Try(Symbol.SlashSlash),
                AnyCharExcept(JassSymbol.CarriageReturnChar, JassSymbol.LineFeedChar).ManyString());
        }
    }
}