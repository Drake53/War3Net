namespace War3Net.CodeAnalysis.VJass.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static TSyntaxNode WithLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, VJassSyntaxTriviaList trivia)
            where TSyntaxNode : VJassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.WithLeadingTrivia(trivia);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode WithTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, VJassSyntaxTriviaList trivia)
            where TSyntaxNode : VJassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.WithTrailingTrivia(trivia);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode PrependTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, VJassSyntaxTriviaList trivia)
            where TSyntaxNode : VJassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.PrependTrivia(trivia);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode AppendTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, VJassSyntaxTriviaList trivia)
            where TSyntaxNode : VJassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.AppendTrivia(trivia);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }
    }
}