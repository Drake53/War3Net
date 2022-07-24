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
            VJassSyntaxToken openParenToken,
            VJassExpressionSyntax expression,
            VJassSyntaxToken closeParenToken)
        {
            OpenParenToken = openParenToken;
            Expression = expression;
            CloseParenToken = closeParenToken;
        }

        public VJassSyntaxToken OpenParenToken { get; }

        public VJassExpressionSyntax Expression { get; }

        public VJassSyntaxToken CloseParenToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassParenthesizedExpressionSyntax parenthesizedExpression
                && Expression.IsEquivalentTo(parenthesizedExpression.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            OpenParenToken.WriteTo(writer);
            Expression.WriteTo(writer);
            CloseParenToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            OpenParenToken.ProcessTo(writer, context);
            Expression.ProcessTo(writer, context);
            CloseParenToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{OpenParenToken}{Expression}{CloseParenToken}";

        public override VJassSyntaxToken GetFirstToken() => OpenParenToken;

        public override VJassSyntaxToken GetLastToken() => CloseParenToken;

        protected internal override VJassParenthesizedExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassParenthesizedExpressionSyntax(
                newToken,
                Expression,
                CloseParenToken);
        }

        protected internal override VJassParenthesizedExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassParenthesizedExpressionSyntax(
                OpenParenToken,
                Expression,
                newToken);
        }
    }
}