// ------------------------------------------------------------------------------
// <copyright file="IdentifierNameFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassIdentifierNameSyntax IdentifierName(JassSyntaxToken identifier)
        {
            if (identifier.SyntaxKind != JassSyntaxKind.IdentifierToken)
            {
                throw new ArgumentException("The token's syntax kind must be 'IdentifierToken'.", nameof(identifier));
            }

            return new JassIdentifierNameSyntax(identifier);
        }

        public static JassIdentifierNameSyntax IdentifierName(string identifierName)
        {
            return new JassIdentifierNameSyntax(Identifier(identifierName));
        }

        public static JassSyntaxToken Identifier(string text)
        {
            return Identifier(JassSyntaxTriviaList.Empty, text, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Identifier(JassSyntaxTriviaList leadingTrivia, string text, JassSyntaxTriviaList trailingTrivia)
        {
            if (!JassSyntaxFacts.IsValidIdentifier(text))
            {
                throw new ArgumentException($"'{text}' is not a valid identifier.", nameof(text));
            }

            return new JassSyntaxToken(leadingTrivia, JassSyntaxKind.IdentifierToken, text, trailingTrivia);
        }
    }
}