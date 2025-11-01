// ------------------------------------------------------------------------------
// <copyright file="JassUnaryExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassUnaryExpressionSyntax : IExpressionSyntax, IJassSyntaxToken
    {
        public JassUnaryExpressionSyntax(UnaryOperatorType @operator, IExpressionSyntax expression)
        {
            Operator = @operator;
            Expression = expression;
        }

        public UnaryOperatorType Operator { get; init; }

        public IExpressionSyntax Expression { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassUnaryExpressionSyntax unaryExpression
                && Operator == unaryExpression.Operator
                && Expression.Equals(unaryExpression.Expression);
        }

        public override string ToString()
        {
            return Operator == UnaryOperatorType.Not
                ? $"{Operator.GetSymbol()} {Expression}"
                : $"{Operator.GetSymbol()}{Expression}";
        }
    }
}