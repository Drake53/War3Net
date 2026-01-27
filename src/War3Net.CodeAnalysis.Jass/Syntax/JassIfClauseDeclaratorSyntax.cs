// ------------------------------------------------------------------------------
// <copyright file="JassIfClauseDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassIfClauseDeclaratorSyntax : JassSyntaxNode
    {
        internal JassIfClauseDeclaratorSyntax(
            JassSyntaxToken ifToken,
            JassExpressionSyntax condition,
            JassSyntaxToken thenToken)
        {
            IfToken = ifToken;
            Condition = condition;
            ThenToken = thenToken;
        }

        public JassSyntaxToken IfToken { get; }

        public JassExpressionSyntax Condition { get; }

        public JassSyntaxToken ThenToken { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.IfClauseDeclarator;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassIfClauseDeclaratorSyntax ifClauseDeclarator
                && Condition.IsEquivalentTo(ifClauseDeclarator.Condition);
        }

        public override void WriteTo(TextWriter writer)
        {
            IfToken.WriteTo(writer);
            Condition.WriteTo(writer);
            ThenToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Condition;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return IfToken;
            yield return ThenToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return IfToken;
            yield return Condition;
            yield return ThenToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Condition;
            foreach (var descendant in Condition.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return IfToken;

            foreach (var descendant in Condition.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return ThenToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return IfToken;

            yield return Condition;
            foreach (var descendant in Condition.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return ThenToken;
        }

        public override string ToString() => $"{IfToken} {Condition} {ThenToken}";

        public override JassSyntaxToken GetFirstToken() => IfToken;

        public override JassSyntaxToken GetLastToken() => ThenToken;

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitIfClauseDeclarator(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitIfClauseDeclarator(this);

        public JassIfClauseDeclaratorSyntax Update(
            JassSyntaxToken ifToken,
            JassExpressionSyntax condition,
            JassSyntaxToken thenToken)
        {
            if (ReferenceEquals(IfToken, ifToken) &&
                ReferenceEquals(Condition, condition) &&
                ReferenceEquals(ThenToken, thenToken))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(ifToken, JassSyntaxKind.IfKeyword);
            ThrowHelper.ThrowIfInvalidToken(thenToken, JassSyntaxKind.ThenKeyword);

            return new JassIfClauseDeclaratorSyntax(ifToken, condition, thenToken);
        }

        public JassIfClauseDeclaratorSyntax WithIfToken(JassSyntaxToken ifToken) => Update(ifToken, Condition, ThenToken);

        public JassIfClauseDeclaratorSyntax WithCondition(JassExpressionSyntax condition) => Update(IfToken, condition, ThenToken);

        public JassIfClauseDeclaratorSyntax WithThenToken(JassSyntaxToken thenToken) => Update(IfToken, Condition, thenToken);

        protected internal override JassIfClauseDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassIfClauseDeclaratorSyntax(
                newToken,
                Condition,
                ThenToken);
        }

        protected internal override JassIfClauseDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassIfClauseDeclaratorSyntax(
                IfToken,
                Condition,
                newToken);
        }
    }
}