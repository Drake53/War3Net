// ------------------------------------------------------------------------------
// <copyright file="JassUnaryExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassUnaryExpressionSyntax : JassExpressionSyntax
    {
        internal JassUnaryExpressionSyntax(
            JassSyntaxToken operatorToken,
            JassExpressionSyntax operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public JassSyntaxToken OperatorToken { get; }

        public JassExpressionSyntax Operand { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxFacts.GetUnaryExpressionKind(OperatorToken.SyntaxKind);

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassUnaryExpressionSyntax unaryExpression
                && OperatorToken.IsEquivalentTo(unaryExpression.OperatorToken)
                && Operand.IsEquivalentTo(unaryExpression.Operand);
        }

        public override void WriteTo(TextWriter writer)
        {
            OperatorToken.WriteTo(writer);
            Operand.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Operand;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return OperatorToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return OperatorToken;
            yield return Operand;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Operand;
            foreach (var descendant in Operand.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return OperatorToken;

            foreach (var descendant in Operand.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return OperatorToken;

            yield return Operand;
            foreach (var descendant in Operand.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{OperatorToken}{(OperatorToken.SyntaxKind == JassSyntaxKind.NotKeyword ? JassSymbol.Space : string.Empty)}{Operand}";

        public override JassSyntaxToken GetFirstToken() => OperatorToken;

        public override JassSyntaxToken GetLastToken() => Operand.GetLastToken();

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitUnaryExpression(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitUnaryExpression(this);

        public JassUnaryExpressionSyntax Update(
            JassSyntaxToken operatorToken,
            JassExpressionSyntax operand)
        {
            if (ReferenceEquals(OperatorToken, operatorToken) &&
                ReferenceEquals(Operand, operand))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidUnaryOperatorToken(operatorToken);

            return new JassUnaryExpressionSyntax(operatorToken, operand);
        }

        public JassUnaryExpressionSyntax WithOperatorToken(JassSyntaxToken operatorToken) => Update(operatorToken, Operand);

        public JassUnaryExpressionSyntax WithOperand(JassExpressionSyntax operand) => Update(OperatorToken, operand);

        protected internal override JassUnaryExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassUnaryExpressionSyntax(
                newToken,
                Operand);
        }

        protected internal override JassUnaryExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassUnaryExpressionSyntax(
                OperatorToken,
                Operand.ReplaceLastToken(newToken));
        }
    }
}