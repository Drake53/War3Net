using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        private const int ArrayWhitespaceDiff = 3;
        private const int PrefixWhitespaceDiff = -1;

        public JassToCSharpTranspiler()
        {
        }

        /// <summary>
        /// Used when <see cref="ApplyCSharpLuaTemplateAttribute"/> is <see langword="true"/>.
        /// </summary>
        public JassToLuaTranspiler? JassToLuaTranspiler { get; set; }

        public bool ApplyCSharpLuaTemplateAttribute { get; set; }

        private SyntaxToken TokenWithSpace(SyntaxKind tokenKind)
        {
            return SyntaxFactory.Token(
                SyntaxTriviaList.Empty,
                tokenKind,
                SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace));
        }

        private SyntaxToken TokenWithSpace(JassSyntaxTriviaList leadingTrivia, SyntaxKind tokenKind)
        {
            return SyntaxFactory.Token(
                Transpile(leadingTrivia),
                tokenKind,
                SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace));
        }

        private SyntaxToken Token(SyntaxKind tokenKind, JassSyntaxTriviaList trailingTrivia)
        {
            return SyntaxFactory.Token(
                SyntaxTriviaList.Empty,
                tokenKind,
                Transpile(trailingTrivia));
        }

        private SyntaxToken Token(SyntaxKind tokenKind, SyntaxTriviaList trailingTrivia)
        {
            return SyntaxFactory.Token(
                SyntaxTriviaList.Empty,
                tokenKind,
                trailingTrivia);
        }

        private JassSyntaxTriviaList MergeTrivia(JassSyntaxToken discardedToken, JassSyntaxTriviaList leadingTrivia)
        {
            if (IsSingleSpace(discardedToken.TrailingTrivia, leadingTrivia))
            {
                return discardedToken.LeadingTrivia;
            }

            return JassSyntaxFactory.ConcatTriviaLists(discardedToken.LeadingTrivia, discardedToken.TrailingTrivia, leadingTrivia);
        }

        private JassSyntaxTriviaList MergeTrivia(JassSyntaxTriviaList trailingTrivia, JassSyntaxToken discardedToken)
        {
            if (IsSingleSpace(trailingTrivia, discardedToken.LeadingTrivia))
            {
                return discardedToken.TrailingTrivia;
            }

            return JassSyntaxFactory.ConcatTriviaLists(trailingTrivia, discardedToken.LeadingTrivia, discardedToken.TrailingTrivia);
        }

        private JassSyntaxTriviaList MergeTrivia(JassSyntaxTriviaList trailingTrivia, JassSyntaxTriviaList leadingTrivia)
        {
            if (IsSingleSpace(trailingTrivia, leadingTrivia))
            {
                return JassSyntaxTriviaList.Empty;
            }

            return JassSyntaxFactory.ConcatTriviaLists(trailingTrivia, leadingTrivia);
        }

        private SyntaxTriviaList MergeTrivia(SyntaxTriviaList trailingTrivia, SyntaxTriviaList leadingTrivia)
        {
            if (IsSingleSpace(trailingTrivia, leadingTrivia))
            {
                return SyntaxTriviaList.Empty;
            }

            return SyntaxFactory.TriviaList(trailingTrivia.Concat(leadingTrivia));
        }

        private bool IsSingleSpace(JassSyntaxTriviaList trailingTrivia, JassSyntaxTriviaList leadingTrivia)
        {
            if (trailingTrivia.Trivia.Length == 1 && leadingTrivia.Trivia.Length == 0)
            {
                return string.Equals(trailingTrivia.Trivia[0].Text, JassSymbol.Space, StringComparison.Ordinal);
            }

            if (trailingTrivia.Trivia.Length == 0 && leadingTrivia.Trivia.Length == 1)
            {
                return string.Equals(leadingTrivia.Trivia[0].Text, JassSymbol.Space, StringComparison.Ordinal);
            }

            return false;
        }

        private bool IsSingleSpace(SyntaxTriviaList trailingTrivia, SyntaxTriviaList leadingTrivia)
        {
            if (trailingTrivia.Count == 1 && leadingTrivia.Count == 0)
            {
                return string.Equals(trailingTrivia[0].ToFullString(), " ", StringComparison.Ordinal);
            }

            if (trailingTrivia.Count == 0 && leadingTrivia.Count == 1)
            {
                return string.Equals(leadingTrivia[0].ToFullString(), " ", StringComparison.Ordinal);
            }

            return false;
        }
    }
}