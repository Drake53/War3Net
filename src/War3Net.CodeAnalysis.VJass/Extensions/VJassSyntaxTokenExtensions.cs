// ------------------------------------------------------------------------------
// <copyright file="VJassSyntaxTokenExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Extensions
{
    public static class VJassSyntaxTokenExtensions
    {
        public static VJassSyntaxToken WithLeadingTrivia(this VJassSyntaxToken token, VJassSyntaxTriviaList trivia)
        {
            return new VJassSyntaxToken(trivia, token.SyntaxKind, token.Text, token.TrailingTrivia);
        }

        public static VJassSyntaxToken WithTrailingTrivia(this VJassSyntaxToken token, VJassSyntaxTriviaList trivia)
        {
            return new VJassSyntaxToken(token.LeadingTrivia, token.SyntaxKind, token.Text, trivia);
        }

        public static VJassSyntaxToken PrependTrivia(this VJassSyntaxToken token, VJassSyntaxTriviaList trivia)
        {
            var builder = trivia.Trivia.ToBuilder();
            builder.AddRange(token.LeadingTrivia.Trivia);
            var newTrivia = new VJassSyntaxTriviaList(builder.ToImmutable());
            return token.WithLeadingTrivia(newTrivia);
        }

        public static VJassSyntaxToken AppendTrivia(this VJassSyntaxToken token, VJassSyntaxTriviaList trivia)
        {
            var builder = token.TrailingTrivia.Trivia.ToBuilder();
            builder.AddRange(trivia.Trivia);
            var newTrivia = new VJassSyntaxTriviaList(builder.ToImmutable());
            return token.WithTrailingTrivia(newTrivia);
        }

        internal static bool NullableEquals(this VJassSyntaxToken? objA, VJassSyntaxToken? objB)
        {
            return (objA is null) == (objB is null);
        }
    }
}