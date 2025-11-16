// ------------------------------------------------------------------------------
// <copyright file="SyntaxNodeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers.Extensions
{
    public static class SyntaxNodeExtensions
    {
        /// <param name="whitespaceDiff">The amount of spaces that must be added (or removed) to keep the syntax aligned.</param>
        public static TSyntax WithAlignedWhitespace<TSyntax>(this TSyntax node, int whitespaceDiff)
            where TSyntax : SyntaxNode
        {
            if (whitespaceDiff == 0 || !node.HasTrailingTrivia)
            {
                return node;
            }

            var trailingWhitespaceTrivia = node.GetTrailingTrivia()[0];
            if (!trailingWhitespaceTrivia.IsKind(SyntaxKind.WhitespaceTrivia))
            {
                return node;
            }

            if (trailingWhitespaceTrivia.Span.Length == 1 ||
                trailingWhitespaceTrivia.ToFullString().Any(c => c != ' '))
            {
                return node;
            }

            var newLength = trailingWhitespaceTrivia.Span.Length + whitespaceDiff;
            if (newLength <= 0)
            {
                newLength = 1;
            }

            return node.ReplaceTrivia(trailingWhitespaceTrivia, SyntaxFactory.Whitespace(new string(' ', newLength)));
        }

        public static TSyntax WithCSharpLuaTemplateAttribute<TSyntax>(this TSyntax node, string template)
            where TSyntax : SyntaxNode
        {
            var newTrivia = SyntaxFactory.Trivia(
                SyntaxFactory.DocumentationCommentTrivia(
                    SyntaxKind.SingleLineDocumentationCommentTrivia,
                    SyntaxFactory.List(new XmlNodeSyntax[]
                    {
                        SyntaxFactory.XmlText(
                            SyntaxFactory.XmlTextLiteral(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.DocumentationCommentExterior("///")),
                                " ",
                                " ",
                                default)),
                        SyntaxFactory.XmlText(
                            $"@CSharpLua.Template = \"{template}\""),
                        SyntaxFactory.XmlText(
                            SyntaxFactory.XmlTextNewLine("\r\n", false)),
                    })));

            if (node.HasLeadingTrivia)
            {
                var newTriviaList = new List<SyntaxTrivia>(2);
                var leadingTrivia = node.GetLeadingTrivia();
                var lastLeadingTrivia = leadingTrivia[^1];
                if (lastLeadingTrivia.IsKind(SyntaxKind.WhitespaceTrivia))
                {
                    newTriviaList.Add(newTrivia);
                    newTriviaList.Add(lastLeadingTrivia);
                }

                return node.InsertTriviaAfter(lastLeadingTrivia, newTriviaList);
            }

            return node.WithLeadingTrivia(newTrivia);
        }
    }
}