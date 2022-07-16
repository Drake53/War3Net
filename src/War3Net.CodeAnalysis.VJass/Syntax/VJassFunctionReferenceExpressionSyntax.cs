// ------------------------------------------------------------------------------
// <copyright file="VJassFunctionReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassFunctionReferenceExpressionSyntax : VJassExpressionSyntax
    {
        internal VJassFunctionReferenceExpressionSyntax(
            VJassSyntaxToken functionToken,
            VJassExpressionSyntax expression)
        {
            FunctionToken = functionToken;
            Expression = expression;
        }

        public VJassSyntaxToken FunctionToken { get; }

        public VJassExpressionSyntax Expression { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassFunctionReferenceExpressionSyntax functionReferenceExpression
                && Expression.IsEquivalentTo(functionReferenceExpression.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            FunctionToken.WriteTo(writer);
            Expression.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            FunctionToken.ProcessTo(writer, context);
            Expression.ProcessTo(writer, context);
        }

        public override string ToString() => $"{FunctionToken} {Expression}";

        public override VJassSyntaxToken GetFirstToken() => FunctionToken;

        public override VJassSyntaxToken GetLastToken() => Expression.GetLastToken();

        protected internal override VJassFunctionReferenceExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassFunctionReferenceExpressionSyntax(
                newToken,
                Expression);
        }

        protected internal override VJassFunctionReferenceExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassFunctionReferenceExpressionSyntax(
                FunctionToken,
                Expression.ReplaceLastToken(newToken));
        }
    }
}