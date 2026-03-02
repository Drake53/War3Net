using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace War3Net.CodeAnalysis.Transpilers.Extensions
{
    public static class SyntaxTokenExtensions
    {
        public static SyntaxToken WithSpace(this SyntaxToken token)
        {
            return token.WithTrailingTrivia(SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace));
        }

        public static SyntaxToken WithoutLeadingTrivia(this SyntaxToken token)
        {
            return token.WithLeadingTrivia(SyntaxTriviaList.Empty);
        }

        public static SyntaxToken WithoutTrailingTrivia(this SyntaxToken token)
        {
            return token.WithTrailingTrivia(SyntaxTriviaList.Empty);
        }
    }
}