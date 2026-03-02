namespace War3Net.CodeAnalysis.Transpilers.Extensions
{
    public static class SeparatedSyntaxListExtensions
    {
        public static SeparatedSyntaxList<TNode> WithoutTrivia<TNode>(this SeparatedSyntaxList<TNode> nodesAndTokens)
            where TNode : SyntaxNode
        {
            if (nodesAndTokens.Count == 0)
            {
                return nodesAndTokens;
            }

            if (nodesAndTokens.Count == 1)
            {
                return SyntaxFactory.SingletonSeparatedList(nodesAndTokens[0].WithoutTrivia());
            }

            var firstNode = nodesAndTokens[0];
            nodesAndTokens = nodesAndTokens.Replace(firstNode, firstNode.WithoutLeadingTrivia());
            var lastNode = nodesAndTokens[^1];
            nodesAndTokens = nodesAndTokens.Replace(lastNode, lastNode.WithoutTrailingTrivia());
            return nodesAndTokens;
        }
    }
}