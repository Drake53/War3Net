// ------------------------------------------------------------------------------
// <copyright file="SyntaxNodeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.VJass.Syntax;

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