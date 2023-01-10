// ------------------------------------------------------------------------------
// <copyright file="JassParenthesizedExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassParenthesizedExpressionSyntax : JassExpressionSyntax
    {
        public JassParenthesizedExpressionSyntax(
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