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
            VJassSyntaxToken openParenthesisToken,
            VJassExpressionSyntax expression,
            VJassSyntaxToken closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public VJassSyntaxToken OpenParenthesisToken { get; }

        public VJassExpressionSyntax Expression { get; }

        public VJassSyntaxToken CloseParenthesisToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassParenthesizedExpressionSyntax parenthesizedExpression
                && Expression.IsEquivalentTo(parenthesizedExpression.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            OpenParenthesisToken.WriteTo(writer);
            Expression.WriteTo(writer);
            CloseParenthesisToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            OpenParenthesisToken.ProcessTo(writer, context);
            Expression.ProcessTo(writer, context);
            CloseParenthesisToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{OpenParenthesisToken}{Expression}{CloseParenthesisToken}";

        public override VJassSyntaxToken GetFirstToken() => OpenParenthesisToken;

        public override VJassSyntaxToken GetLastToken() => CloseParenthesisToken;

        protected internal override VJassParenthesizedExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassParenthesizedExpressionSyntax(
                newToken,
                Expression,
                CloseParenthesisToken);
        }

        protected internal override VJassParenthesizedExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassParenthesizedExpressionSyntax(
                OpenParenthesisToken,
                Expression,
                newToken);
        }
    }
}