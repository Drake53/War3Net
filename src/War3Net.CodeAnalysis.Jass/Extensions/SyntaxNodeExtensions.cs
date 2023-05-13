// ------------------------------------------------------------------------------
// <copyright file="SyntaxNodeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static TSyntaxNode WithLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTriviaList triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.WithLeadingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode WithTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTriviaList triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.WithTrailingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode AppendLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTrivia trivia)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.AppendLeadingTrivia(trivia);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode AppendLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTriviaList triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.AppendLeadingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode AppendLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, params JassSyntaxTrivia[] triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.AppendLeadingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode AppendLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, IEnumerable<JassSyntaxTrivia> triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.AppendLeadingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode AppendLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, ImmutableArray<JassSyntaxTrivia> triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.AppendLeadingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode PrependLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTrivia trivia)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.PrependLeadingTrivia(trivia);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode PrependLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTriviaList triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.PrependLeadingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode PrependLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, params JassSyntaxTrivia[] triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.PrependLeadingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode PrependLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, IEnumerable<JassSyntaxTrivia> triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.PrependLeadingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode PrependLeadingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, ImmutableArray<JassSyntaxTrivia> triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetFirstToken();
            var newToken = oldToken.PrependLeadingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceFirstToken(newToken);
        }

        public static TSyntaxNode AppendTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTrivia trivia)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.AppendTrailingTrivia(trivia);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode AppendTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTriviaList triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.AppendTrailingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode AppendTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, params JassSyntaxTrivia[] triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.AppendTrailingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode AppendTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, IEnumerable<JassSyntaxTrivia> triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.AppendTrailingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode AppendTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, ImmutableArray<JassSyntaxTrivia> triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.AppendTrailingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode PrependTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTrivia trivia)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.PrependTrailingTrivia(trivia);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode PrependTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, JassSyntaxTriviaList triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.PrependTrailingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode PrependTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, params JassSyntaxTrivia[] triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.PrependTrailingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode PrependTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, IEnumerable<JassSyntaxTrivia> triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.PrependTrailingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }

        public static TSyntaxNode PrependTrailingTrivia<TSyntaxNode>(this TSyntaxNode syntaxNode, ImmutableArray<JassSyntaxTrivia> triviaList)
            where TSyntaxNode : JassSyntaxNode
        {
            var oldToken = syntaxNode.GetLastToken();
            var newToken = oldToken.PrependTrailingTrivia(triviaList);
            return (TSyntaxNode)syntaxNode.ReplaceLastToken(newToken);
        }
    }
}