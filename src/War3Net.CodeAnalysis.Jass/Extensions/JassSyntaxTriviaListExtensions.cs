using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassSyntaxTriviaListExtensions
    {
        public static string GetIndentationString(this JassSyntaxTriviaList triviaList)
        {
            if (triviaList.Trivia.IsEmpty)
            {
                return string.Empty;
            }

            var lastTrivia = triviaList.Trivia[^1];
            if (lastTrivia.SyntaxKind != JassSyntaxKind.WhitespaceTrivia)
            {
                return string.Empty;
            }

            return lastTrivia.Text;
        }
    }
}