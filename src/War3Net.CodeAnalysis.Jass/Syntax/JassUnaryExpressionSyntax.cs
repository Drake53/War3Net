// ------------------------------------------------------------------------------
// <copyright file="JassUnaryExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

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