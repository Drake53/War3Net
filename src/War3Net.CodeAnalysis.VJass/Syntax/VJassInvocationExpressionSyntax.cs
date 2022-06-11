// ------------------------------------------------------------------------------
// <copyright file="VJassInvocationExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassInvocationExpressionSyntax : VJassExpressionSyntax
    {
        internal VJassInvocationExpressionSyntax(
            VJassExpressionSyntax expression,
            VJassArgumentListSyntax argumentList)
        {
            Expression = expression;
            ArgumentList = argumentList;
        }

        public VJassExpressionSyntax Expression { get; }

        public VJassArgumentListSyntax ArgumentList { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassInvocationExpressionSyntax invocationExpression
                && Expression.IsEquivalentTo(invocationExpression.Expression)
                && ArgumentList.IsEquivalentTo(invocationExpression.ArgumentList);
        }

        public override void WriteTo(TextWriter writer)
        {
            Expression.WriteTo(writer);
            ArgumentList.WriteTo(writer);
        }

        public override string ToString() => $"{Expression}{ArgumentList}";

        public override VJassSyntaxToken GetFirstToken() => Expression.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => ArgumentList.GetLastToken();

        protected internal override VJassInvocationExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassInvocationExpressionSyntax(
                Expression.ReplaceFirstToken(newToken),
                ArgumentList);
        }

        protected internal override VJassInvocationExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassInvocationExpressionSyntax(
                Expression,
                ArgumentList.ReplaceLastToken(newToken));
        }
    }
}