// ------------------------------------------------------------------------------
// <copyright file="JassEqualsValueClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEqualsValueClauseSyntax : JassSyntaxNode
    {
        internal JassEqualsValueClauseSyntax(
            JassSyntaxToken equalsToken,
            JassExpressionSyntax value)
        {
            EqualsToken = equalsToken;
            Value = value;
        }

        public JassSyntaxToken EqualsToken { get; }

        public JassExpressionSyntax Value { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.EqualsValueClause;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassEqualsValueClauseSyntax equalsValueClause
                && Value.IsEquivalentTo(equalsValueClause.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            EqualsToken.WriteTo(writer);
            Value.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Value;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return EqualsToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return EqualsToken;
            yield return Value;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Value;
            foreach (var descendant in Value.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return EqualsToken;

            foreach (var descendant in Value.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return EqualsToken;

            yield return Value;
            foreach (var descendant in Value.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{EqualsToken} {Value}";

        public override JassSyntaxToken GetFirstToken() => EqualsToken;

        public override JassSyntaxToken GetLastToken() => Value.GetLastToken();

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitEqualsValueClause(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitEqualsValueClause(this);

        public JassEqualsValueClauseSyntax Update(
            JassSyntaxToken equalsToken,
            JassExpressionSyntax value)
        {
            if (ReferenceEquals(EqualsToken, equalsToken) &&
                ReferenceEquals(Value, value))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(equalsToken, JassSyntaxKind.EqualsToken);

            return new JassEqualsValueClauseSyntax(equalsToken, value);
        }

        public JassEqualsValueClauseSyntax WithEqualsToken(JassSyntaxToken equalsToken) => Update(equalsToken, Value);

        public JassEqualsValueClauseSyntax WithValue(JassExpressionSyntax value) => Update(EqualsToken, value);

        protected internal override JassEqualsValueClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassEqualsValueClauseSyntax(
                newToken,
                Value);
        }

        protected internal override JassEqualsValueClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassEqualsValueClauseSyntax(
                EqualsToken,
                Value.ReplaceLastToken(newToken));
        }
    }
}