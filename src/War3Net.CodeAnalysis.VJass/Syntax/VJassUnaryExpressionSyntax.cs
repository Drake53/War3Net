// ------------------------------------------------------------------------------
// <copyright file="VJassUnaryExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassUnaryExpressionSyntax : VJassExpressionSyntax
    {
        public VJassUnaryExpressionSyntax(
            VJassSyntaxToken operatorToken,
            VJassExpressionSyntax expression)
        {
            OperatorToken = operatorToken;
            Expression = expression;
        }

        public VJassSyntaxToken OperatorToken { get; }

        public VJassExpressionSyntax Expression { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassUnaryExpressionSyntax unaryExpression
                && OperatorToken.IsEquivalentTo(unaryExpression.OperatorToken)
                && Expression.IsEquivalentTo(unaryExpression.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            OperatorToken.WriteTo(writer);
            Expression.WriteTo(writer);
        }

        public override string ToString() => $"{OperatorToken}{(OperatorToken.SyntaxKind == VJassSyntaxKind.NotKeyword ? " " : string.Empty)}{Expression}";

        public override VJassSyntaxToken GetFirstToken() => OperatorToken;

        public override VJassSyntaxToken GetLastToken() => Expression.GetLastToken();

        protected internal override VJassUnaryExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassUnaryExpressionSyntax(
                newToken,
                Expression);
        }

        protected internal override VJassUnaryExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassUnaryExpressionSyntax(
                OperatorToken,
                Expression.ReplaceLastToken(newToken));
        }
    }
}