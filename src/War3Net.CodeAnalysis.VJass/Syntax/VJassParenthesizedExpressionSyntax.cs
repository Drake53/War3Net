// ------------------------------------------------------------------------------
// <copyright file="VJassParenthesizedExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassParenthesizedExpressionSyntax : VJassExpressionSyntax
    {
        public VJassParenthesizedExpressionSyntax(
            VJassSyntaxToken leftParenthesisToken,
            VJassExpressionSyntax expression,
            VJassSyntaxToken rightParenthesisToken)
        {
            LeftParenthesisToken = leftParenthesisToken;
            Expression = expression;
            RightParenthesisToken = rightParenthesisToken;
        }

        public VJassSyntaxToken LeftParenthesisToken { get; }

        public VJassExpressionSyntax Expression { get; }

        public VJassSyntaxToken RightParenthesisToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassParenthesizedExpressionSyntax parenthesizedExpression
                && Expression.IsEquivalentTo(parenthesizedExpression.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            LeftParenthesisToken.WriteTo(writer);
            Expression.WriteTo(writer);
            RightParenthesisToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            LeftParenthesisToken.ProcessTo(writer, context);
            Expression.ProcessTo(writer, context);
            RightParenthesisToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{LeftParenthesisToken}{Expression}{RightParenthesisToken}";

        public override VJassSyntaxToken GetFirstToken() => LeftParenthesisToken;

        public override VJassSyntaxToken GetLastToken() => RightParenthesisToken;

        protected internal override VJassParenthesizedExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassParenthesizedExpressionSyntax(
                newToken,
                Expression,
                RightParenthesisToken);
        }

        protected internal override VJassParenthesizedExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassParenthesizedExpressionSyntax(
                LeftParenthesisToken,
                Expression,
                newToken);
        }
    }
}