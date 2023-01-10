// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxTokenExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassSyntaxTokenExtensions
    {
        public static JassSyntaxToken WithLeadingTrivia(this JassSyntaxToken token, JassSyntaxTriviaList trivia)
        {
            return new JassSyntaxToken(trivia, token.SyntaxKind, token.Text, token.TrailingTrivia);
        }

        public static JassSyntaxToken WithTrailingTrivia(this JassSyntaxToken token, JassSyntaxTriviaList trivia)
        {
            return new JassSyntaxToken(token.LeadingTrivia, token.SyntaxKind, token.Text, trivia);
        }

        public static JassSyntaxToken PrependTrivia(this JassSyntaxToken token, JassSyntaxTriviaList trivia)
        {
            var builder = trivia.Trivia.ToBuilder();
            builder.AddRange(token.LeadingTrivia.Trivia);
            var newTrivia = new JassSyntaxTriviaList(builder.ToImmutable());
            return token.WithLeadingTrivia(newTrivia);
        }

        public static JassSyntaxToken AppendTrivia(this JassSyntaxToken token, JassSyntaxTriviaList trivia)
        {
            var builder = token.TrailingTrivia.Trivia.ToBuilder();
            builder.AddRange(trivia.Trivia);
            var newTrivia = new JassSyntaxTriviaList(builder.ToImmutable());
            return token.WithTrailingTrivia(newTrivia);
        }

        internal static bool NullableEquals(this JassSyntaxToken? objA, JassSyntaxToken? objB)
        {
            return (objA is null) == (objB is null);
        }
    }
}