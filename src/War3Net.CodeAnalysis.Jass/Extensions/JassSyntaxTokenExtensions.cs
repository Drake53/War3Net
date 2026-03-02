using System.Collections.Generic;
using System.Collections.Immutable;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassSyntaxTokenExtensions
    {
        public static JassSyntaxToken WithLeadingTrivia(this JassSyntaxToken token, JassSyntaxTriviaList triviaList)
        {
            return new JassSyntaxToken(triviaList, token.SyntaxKind, token.Text, token.TrailingTrivia);
        }

        public static JassSyntaxToken WithTrailingTrivia(this JassSyntaxToken token, JassSyntaxTriviaList triviaList)
        {
            return new JassSyntaxToken(token.LeadingTrivia, token.SyntaxKind, token.Text, triviaList);
        }

        public static JassSyntaxToken AppendLeadingTrivia(this JassSyntaxToken token, JassSyntaxTrivia trivia)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.AppendTriviaToList(token.LeadingTrivia.Trivia, trivia));
        }

        public static JassSyntaxToken AppendLeadingTrivia(this JassSyntaxToken token, JassSyntaxTriviaList triviaList)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.ConcatTriviaLists(token.LeadingTrivia.Trivia, triviaList.Trivia));
        }

        public static JassSyntaxToken AppendLeadingTrivia(this JassSyntaxToken token, params JassSyntaxTrivia[] triviaList)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.ConcatTriviaLists(token.LeadingTrivia.Trivia, triviaList));
        }

        public static JassSyntaxToken AppendLeadingTrivia(this JassSyntaxToken token, IEnumerable<JassSyntaxTrivia> triviaList)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.ConcatTriviaLists(token.LeadingTrivia.Trivia, triviaList));
        }

        public static JassSyntaxToken AppendLeadingTrivia(this JassSyntaxToken token, ImmutableArray<JassSyntaxTrivia> triviaList)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.ConcatTriviaLists(token.LeadingTrivia.Trivia, triviaList));
        }

        public static JassSyntaxToken PrependLeadingTrivia(this JassSyntaxToken token, JassSyntaxTrivia trivia)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.PrependTriviaToList(trivia, token.LeadingTrivia.Trivia));
        }

        public static JassSyntaxToken PrependLeadingTrivia(this JassSyntaxToken token, JassSyntaxTriviaList triviaList)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.ConcatTriviaLists(triviaList.Trivia, token.LeadingTrivia.Trivia));
        }

        public static JassSyntaxToken PrependLeadingTrivia(this JassSyntaxToken token, params JassSyntaxTrivia[] triviaList)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.ConcatTriviaLists(triviaList, token.LeadingTrivia.Trivia));
        }

        public static JassSyntaxToken PrependLeadingTrivia(this JassSyntaxToken token, IEnumerable<JassSyntaxTrivia> triviaList)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.ConcatTriviaLists(triviaList, token.LeadingTrivia.Trivia));
        }

        public static JassSyntaxToken PrependLeadingTrivia(this JassSyntaxToken token, ImmutableArray<JassSyntaxTrivia> triviaList)
        {
            return token.WithLeadingTrivia(JassSyntaxFactory.ConcatTriviaLists(triviaList, token.LeadingTrivia.Trivia));
        }

        public static JassSyntaxToken AppendTrailingTrivia(this JassSyntaxToken token, JassSyntaxTrivia trivia)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.AppendTriviaToList(token.TrailingTrivia.Trivia, trivia));
        }

        public static JassSyntaxToken AppendTrailingTrivia(this JassSyntaxToken token, JassSyntaxTriviaList triviaList)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.ConcatTriviaLists(token.TrailingTrivia.Trivia, triviaList.Trivia));
        }

        public static JassSyntaxToken AppendTrailingTrivia(this JassSyntaxToken token, params JassSyntaxTrivia[] triviaList)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.ConcatTriviaLists(token.TrailingTrivia.Trivia, triviaList));
        }

        public static JassSyntaxToken AppendTrailingTrivia(this JassSyntaxToken token, IEnumerable<JassSyntaxTrivia> triviaList)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.ConcatTriviaLists(token.TrailingTrivia.Trivia, triviaList));
        }

        public static JassSyntaxToken AppendTrailingTrivia(this JassSyntaxToken token, ImmutableArray<JassSyntaxTrivia> triviaList)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.ConcatTriviaLists(token.TrailingTrivia.Trivia, triviaList));
        }

        public static JassSyntaxToken PrependTrailingTrivia(this JassSyntaxToken token, JassSyntaxTrivia trivia)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.PrependTriviaToList(trivia, token.TrailingTrivia.Trivia));
        }

        public static JassSyntaxToken PrependTrailingTrivia(this JassSyntaxToken token, JassSyntaxTriviaList triviaList)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.ConcatTriviaLists(triviaList.Trivia, token.TrailingTrivia.Trivia));
        }

        public static JassSyntaxToken PrependTrailingTrivia(this JassSyntaxToken token, params JassSyntaxTrivia[] triviaList)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.ConcatTriviaLists(triviaList, token.TrailingTrivia.Trivia));
        }

        public static JassSyntaxToken PrependTrailingTrivia(this JassSyntaxToken token, IEnumerable<JassSyntaxTrivia> triviaList)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.ConcatTriviaLists(triviaList, token.TrailingTrivia.Trivia));
        }

        public static JassSyntaxToken PrependTrailingTrivia(this JassSyntaxToken token, ImmutableArray<JassSyntaxTrivia> triviaList)
        {
            return token.WithTrailingTrivia(JassSyntaxFactory.ConcatTriviaLists(triviaList, token.TrailingTrivia.Trivia));
        }

        internal static bool NullableEquals(this JassSyntaxToken? objA, JassSyntaxToken? objB)
        {
            return (objA is null) == (objB is null);
        }
    }
}