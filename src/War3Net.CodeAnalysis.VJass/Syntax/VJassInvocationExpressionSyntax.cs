// ------------------------------------------------------------------------------
// <copyright file="VJassInvocationExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassInvocationExpressionSyntax : IExpressionSyntax
    {
        public VJassInvocationExpressionSyntax(
            IExpressionSyntax expression,
            VJassArgumentListSyntax arguments)
        {
            Expression = expression;
            Arguments = arguments;
        }

        public IExpressionSyntax Expression { get; }

        public VJassArgumentListSyntax Arguments { get; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is VJassInvocationExpressionSyntax invocationExpression
                && Expression.Equals(invocationExpression.Expression)
                && Arguments.Equals(invocationExpression.Arguments);
        }

        public override string ToString() => $"{Expression}{VJassSymbol.LeftParenthesis}{Arguments}{VJassSymbol.RightParenthesis}";
    }
}