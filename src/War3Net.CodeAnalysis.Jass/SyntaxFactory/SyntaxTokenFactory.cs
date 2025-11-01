// ------------------------------------------------------------------------------
// <copyright file="SyntaxTokenFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        private static readonly Dictionary<JassSyntaxKind, JassSyntaxToken> _defaultTokens = new();

        public static JassSyntaxToken Token(JassSyntaxKind syntaxKind)
        {
            if (_defaultTokens.TryGetValue(syntaxKind, out var token))
            {
                return token;
            }

            var text = JassSyntaxFacts.GetText(syntaxKind);
            if (text.Length == 0 && syntaxKind != JassSyntaxKind.EndOfFileToken)
            {
                throw new InvalidEnumArgumentException(nameof(syntaxKind), (int)syntaxKind, typeof(JassSyntaxKind));
            }

            token = new JassSyntaxToken(JassSyntaxTriviaList.Empty, syntaxKind, text, JassSyntaxTriviaList.Empty);
            _defaultTokens.Add(syntaxKind, token);
            return token;
        }

        public static JassSyntaxToken Token(JassSyntaxTriviaList leadingTrivia, JassSyntaxKind syntaxKind)
        {
            return Token(leadingTrivia, syntaxKind, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Token(JassSyntaxKind syntaxKind, JassSyntaxTriviaList trailingTrivia)
        {
            return Token(JassSyntaxTriviaList.Empty, syntaxKind, trailingTrivia);
        }

        public static JassSyntaxToken Token(JassSyntaxKind syntaxKind, string text)
        {
            return Token(JassSyntaxTriviaList.Empty, syntaxKind, text, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Token(JassSyntaxTriviaList leadingTrivia, JassSyntaxKind syntaxKind, JassSyntaxTriviaList trailingTrivia)
        {
            var text = JassSyntaxFacts.GetText(syntaxKind);
            if (text.Length == 0 && syntaxKind != JassSyntaxKind.EndOfFileToken)
            {
                throw new InvalidEnumArgumentException(nameof(syntaxKind), (int)syntaxKind, typeof(JassSyntaxKind));
            }

            return new JassSyntaxToken(leadingTrivia, syntaxKind, text, trailingTrivia);
        }

        public static JassSyntaxToken Token(JassSyntaxTriviaList leadingTrivia, JassSyntaxKind syntaxKind, string text, JassSyntaxTriviaList trailingTrivia)
        {
            var defaultText = JassSyntaxFacts.GetText(syntaxKind);
            if (defaultText.Length == 0 && syntaxKind != JassSyntaxKind.EndOfFileToken)
            {
                var syntaxKindForText = JassSyntaxFacts.GetSyntaxKind(text);
                if (syntaxKindForText != JassSyntaxKind.None)
                {
                    throw new ArgumentException($"Text '{text}' is not valid for a token of type '{syntaxKind}', because this text is reserved for tokens of type '{syntaxKindForText}'.", nameof(text));
                }
            }
            else if (!string.Equals(text, defaultText, StringComparison.Ordinal))
            {
                throw new ArgumentException($"Text '{text}' is not valid for a token of type '{syntaxKind}', because the text for this type must be '{defaultText}'.", nameof(text));
            }

            return new JassSyntaxToken(leadingTrivia, syntaxKind, text, trailingTrivia);
        }
    }
}