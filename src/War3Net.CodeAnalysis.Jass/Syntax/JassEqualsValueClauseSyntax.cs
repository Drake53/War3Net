// ------------------------------------------------------------------------------
// <copyright file="JassEqualsValueClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using OneOf;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEqualsValueClauseSyntax : JassSyntaxNode
    {
        internal JassEqualsValueClauseSyntax(
            JassSyntaxToken equalsToken,
            JassExpressionSyntax expression)
        {
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public JassSyntaxToken EqualsToken { get; }

        public JassExpressionSyntax Expression { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassEqualsValueClauseSyntax equalsValueClause
                && Expression.IsEquivalentTo(equalsValueClause.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            EqualsToken.WriteTo(writer);
            Expression.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Expression;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return EqualsToken;
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetChildNodesAndTokens()
        {
            yield return EqualsToken;
            yield return Expression;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Expression;
            foreach (var descendant in Expression.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return EqualsToken;

            foreach (var descendant in Expression.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens()
        {
            yield return EqualsToken;

            yield return Expression;
            foreach (var descendant in Expression.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{EqualsToken} {Expression}";

        public override JassSyntaxToken GetFirstToken() => EqualsToken;

        public override JassSyntaxToken GetLastToken() => Expression.GetLastToken();

        protected internal override JassEqualsValueClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassEqualsValueClauseSyntax(
                newToken,
                Expression);
        }

        protected internal override JassEqualsValueClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassEqualsValueClauseSyntax(
                EqualsToken,
                Expression.ReplaceLastToken(newToken));
        }
    }
}