// ------------------------------------------------------------------------------
// <copyright file="SyntaxTriviaFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassSyntaxTrivia SyntaxTrivia(JassSyntaxKind syntaxKind, string text)
        {
            return syntaxKind switch
            {
                JassSyntaxKind.NewlineTrivia => NewlineTrivia(text),
                JassSyntaxKind.WhitespaceTrivia => WhitespaceTrivia(text),
                JassSyntaxKind.SingleLineCommentTrivia => SingleLineCommentTrivia(text),

                _ => throw new InvalidEnumArgumentException(nameof(syntaxKind), (int)syntaxKind, typeof(JassSyntaxKind)),
            };
        }

        public static JassSyntaxTrivia NewlineTrivia(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (!text.All(c => c == JassSymbol.CarriageReturnChar || c == JassSymbol.LineFeedChar))
            {
                throw new ArgumentException("Text may only contain '\\r' and '\\n' characters.", nameof(text));
            }

            return new JassSyntaxTrivia(JassSyntaxKind.NewlineTrivia, text);
        }

        public static JassSyntaxTrivia WhitespaceTrivia(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (!text.All(JassSyntaxFacts.IsWhitespaceCharacter))
            {
                throw new ArgumentException("Text may only contain whitespace characters (excluding '\\r' and '\\n').", nameof(text));
            }

            return new JassSyntaxTrivia(JassSyntaxKind.WhitespaceTrivia, text);
        }

        public static JassSyntaxTrivia SingleLineCommentTrivia(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (!text.StartsWith(JassSymbol.SlashSlash, false, CultureInfo.InvariantCulture))
            {
                throw new ArgumentException("Text must start with \"//\".", nameof(text));
            }

            if (text.Any(c => c == JassSymbol.CarriageReturnChar || c == JassSymbol.LineFeedChar))
            {
                throw new ArgumentException("Text may not contain '\\r' or '\\n' characters.", nameof(text));
            }

            return new JassSyntaxTrivia(JassSyntaxKind.SingleLineCommentTrivia, text);
        }
    }
}