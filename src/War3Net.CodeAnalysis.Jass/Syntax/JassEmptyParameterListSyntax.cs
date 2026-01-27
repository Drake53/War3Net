// ------------------------------------------------------------------------------
// <copyright file="JassEmptyParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEmptyParameterListSyntax : JassParameterListOrEmptyParameterListSyntax
    {
        public static readonly JassEmptyParameterListSyntax Value = new(
            new JassSyntaxToken(JassSyntaxKind.TakesKeyword, JassKeyword.Takes, JassSyntaxTriviaList.SingleSpace),
            new JassSyntaxToken(JassSyntaxKind.NothingKeyword, JassKeyword.Nothing, JassSyntaxTriviaList.SingleSpace));

        internal JassEmptyParameterListSyntax(
            JassSyntaxToken takesToken,
            JassSyntaxToken nothingToken)
        {
            TakesToken = takesToken;
            NothingToken = nothingToken;
        }

        public override JassSyntaxToken TakesToken { get; }

        public JassSyntaxToken NothingToken { get; }

        public override SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> Parameters => SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken>.Empty;

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.EmptyParameterList;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassEmptyParameterListSyntax;
        }

        public override void WriteTo(TextWriter writer)
        {
            TakesToken.WriteTo(writer);
            NothingToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return TakesToken;
            yield return NothingToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return TakesToken;
            yield return NothingToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return TakesToken;
            yield return NothingToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return TakesToken;
            yield return NothingToken;
        }

        public override string ToString() => $"{TakesToken} {NothingToken}";

        public override JassSyntaxToken GetFirstToken() => TakesToken;

        public override JassSyntaxToken GetLastToken() => NothingToken;

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitEmptyParameterList(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitEmptyParameterList(this);

        public JassEmptyParameterListSyntax Update(
            JassSyntaxToken takesToken,
            JassSyntaxToken nothingToken)
        {
            if (ReferenceEquals(TakesToken, takesToken) &&
                ReferenceEquals(NothingToken, nothingToken))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(takesToken, JassSyntaxKind.TakesKeyword);
            ThrowHelper.ThrowIfInvalidToken(nothingToken, JassSyntaxKind.NothingKeyword);

            return new JassEmptyParameterListSyntax(takesToken, nothingToken);
        }

        public JassEmptyParameterListSyntax WithTakesToken(JassSyntaxToken takesToken) => Update(takesToken, NothingToken);

        public JassEmptyParameterListSyntax WithNothingToken(JassSyntaxToken nothingToken) => Update(TakesToken, nothingToken);

        protected internal override JassEmptyParameterListSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassEmptyParameterListSyntax(
                newToken,
                NothingToken);
        }

        protected internal override JassEmptyParameterListSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassEmptyParameterListSyntax(
                TakesToken,
                newToken);
        }
    }
}