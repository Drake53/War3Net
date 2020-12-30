// ------------------------------------------------------------------------------
// <copyright file="JassParenthesizedExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassParenthesizedExpressionSyntax : IExpressionSyntax
    {
        public JassParenthesizedExpressionSyntax(IExpressionSyntax expression)
        {
            Expression = expression;
        }

        public IExpressionSyntax Expression { get; init; }

        public bool Equals(IExpressionSyntax? other) => other is JassParenthesizedExpressionSyntax e && Expression.Equals(e.Expression);

        public override string ToString() => $"({Expression})";
    }
}