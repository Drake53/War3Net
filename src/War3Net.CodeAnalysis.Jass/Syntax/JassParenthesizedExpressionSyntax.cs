// ------------------------------------------------------------------------------
// <copyright file="JassParenthesizedExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassParenthesizedExpressionSyntax : JassExpressionSyntax
    {
        internal JassParenthesizedExpressionSyntax(
            JassSyntaxToken openParenToken,
            JassExpressionSyntax expression,
            JassSyntaxToken closeParenToken)
        {
            OpenParenToken = openParenToken;
            Expression = expression;
            CloseParenToken = closeParenToken;
        }

        public JassSyntaxToken OpenParenToken { get; }

        public JassExpressionSyntax Expression { get; }

        public JassSyntaxToken CloseParenToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassParenthesizedExpressionSyntax parenthesizedExpression
                && Expression.IsEquivalentTo(parenthesizedExpression.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            OpenParenToken.WriteTo(writer);
            Expression.WriteTo(writer);
            CloseParenToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Expression;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return OpenParenToken;
            yield return CloseParenToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return OpenParenToken;
            yield return Expression;
            yield return CloseParenToken;
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
            yield return OpenParenToken;

            foreach (var descendant in Expression.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return CloseParenToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return OpenParenToken;

            yield return Expression;
            foreach (var descendant in Expression.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return CloseParenToken;
        }

        public override string ToString() => $"{OpenParenToken}{Expression}{CloseParenToken}";

        public override JassSyntaxToken GetFirstToken() => OpenParenToken;

        public override JassSyntaxToken GetLastToken() => CloseParenToken;

        protected internal override JassParenthesizedExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassParenthesizedExpressionSyntax(
                newToken,
                Expression,
                CloseParenToken);
        }

        protected internal override JassParenthesizedExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassParenthesizedExpressionSyntax(
                OpenParenToken,
                Expression,
                newToken);
        }
    }
}