namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public SyntaxTriviaList Transpile(JassSyntaxTriviaList triviaList)
        {
            if (triviaList.Trivia.Length == 0)
            {
                return SyntaxTriviaList.Empty;
            }

            return SyntaxFactory.TriviaList(triviaList.Trivia.Select(Transpile));
        }

        public SyntaxTrivia Transpile(JassSyntaxTrivia trivia)
        {
            return trivia.SyntaxKind switch
            {
                JassSyntaxKind.NewLineTrivia => SyntaxFactory.EndOfLine(trivia.Text),
                JassSyntaxKind.WhitespaceTrivia => SyntaxFactory.Whitespace(trivia.Text),
                JassSyntaxKind.SingleLineCommentTrivia => SyntaxFactory.Comment(trivia.Text),
            };
        }
    }
}