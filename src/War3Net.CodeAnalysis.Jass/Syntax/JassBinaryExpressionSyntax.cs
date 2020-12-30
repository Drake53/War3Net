// ------------------------------------------------------------------------------
// <copyright file="JassBinaryExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassBinaryExpressionSyntax : IExpressionSyntax
    {
        public JassBinaryExpressionSyntax(BinaryOperatorType @operator, IExpressionSyntax left, IExpressionSyntax right)
        {
            Operator = @operator;
            Left = left;
            Right = right;
        }

        public BinaryOperatorType Operator { get; init; }

        public IExpressionSyntax Left { get; init; }

        public IExpressionSyntax Right { get; init; }

        public bool Equals(IExpressionSyntax? other) => other is JassBinaryExpressionSyntax e && Operator == e.Operator && Left.Equals(e.Left) && Right.Equals(e.Right);

        public override string ToString() => $"{Left} {Operator.GetString()} {Right}";
    }
}