// ------------------------------------------------------------------------------
// <copyright file="JassUnaryExpressionSyntax.cs" company="Drake53">
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
    public class JassUnaryExpressionSyntax : JassExpressionSyntax
    {
        public JassUnaryExpressionSyntax(
            JassSyntaxToken operatorToken,
            JassExpressionSyntax expression)
        {
            OperatorToken = operatorToken;
            Expression = expression;
        }

        public JassSyntaxToken OperatorToken { get; }

        public JassExpressionSyntax Expression { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassUnaryExpressionSyntax unaryExpression
                && OperatorToken.IsEquivalentTo(unaryExpression.OperatorToken)
                && Expression.IsEquivalentTo(unaryExpression.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            OperatorToken.WriteTo(writer);
            Expression.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Expression;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return OperatorToken;
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetChildNodesAndTokens()
        {
            yield return OperatorToken;
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
            yield return OperatorToken;

            foreach (var descendant in Expression.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens()
        {
            yield return OperatorToken;

            yield return Expression;
            foreach (var descendant in Expression.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{OperatorToken}{(OperatorToken.SyntaxKind == JassSyntaxKind.NotKeyword ? " " : string.Empty)}{Expression}";

        public override JassSyntaxToken GetFirstToken() => OperatorToken;

        public override JassSyntaxToken GetLastToken() => Expression.GetLastToken();

        protected internal override JassUnaryExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassUnaryExpressionSyntax(
                newToken,
                Expression);
        }

        protected internal override JassUnaryExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassUnaryExpressionSyntax(
                OperatorToken,
                Expression.ReplaceLastToken(newToken));
        }
    }
}