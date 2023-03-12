// ------------------------------------------------------------------------------
// <copyright file="JassArgumentListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using OneOf;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassArgumentListSyntax : JassSyntaxNode
    {
        public static readonly JassArgumentListSyntax Empty = new(
            new JassSyntaxToken(JassSyntaxKind.OpenParenToken, JassSymbol.OpenParen, JassSyntaxTriviaList.Empty),
            SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.Empty,
            new JassSyntaxToken(JassSyntaxKind.CloseParenToken, JassSymbol.CloseParen, JassSyntaxTriviaList.Empty));

        internal JassArgumentListSyntax(
            JassSyntaxToken openParenToken,
            SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken> argumentList,
            JassSyntaxToken closeParenToken)
        {
            OpenParenToken = openParenToken;
            ArgumentList = argumentList;
            CloseParenToken = closeParenToken;
        }

        public JassSyntaxToken OpenParenToken { get; }

        public SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken> ArgumentList { get; }

        public JassSyntaxToken CloseParenToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassArgumentListSyntax argumentList
                && ArgumentList.IsEquivalentTo(argumentList.ArgumentList);
        }

        public override void WriteTo(TextWriter writer)
        {
            OpenParenToken.WriteTo(writer);
            ArgumentList.WriteTo(writer);
            CloseParenToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            return ArgumentList.Items;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return OpenParenToken;
            foreach (var child in ArgumentList.Separators)
            {
                yield return child;
            }

            yield return CloseParenToken;
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetChildNodesAndTokens()
        {
            yield return OpenParenToken;
            foreach (var child in ArgumentList.GetChildNodesAndTokens())
            {
                yield return child;
            }

            yield return CloseParenToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            return ArgumentList.GetDescendantNodes();
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return OpenParenToken;
            foreach (var descendant in ArgumentList.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return CloseParenToken;
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens()
        {
            yield return OpenParenToken;
            foreach (var descendant in ArgumentList.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return CloseParenToken;
        }

        public override string ToString() => $"{OpenParenToken}{ArgumentList}{CloseParenToken}";

        public override JassSyntaxToken GetFirstToken() => OpenParenToken;

        public override JassSyntaxToken GetLastToken() => CloseParenToken;

        protected internal override JassArgumentListSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassArgumentListSyntax(
                newToken,
                ArgumentList,
                CloseParenToken);
        }

        protected internal override JassArgumentListSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassArgumentListSyntax(
                OpenParenToken,
                ArgumentList,
                newToken);
        }
    }
}