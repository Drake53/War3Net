// ------------------------------------------------------------------------------
// <copyright file="JassBooleanLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassBooleanLiteralExpressionSyntax : IExpressionSyntax, IJassSyntaxToken
    {
        public static readonly JassBooleanLiteralExpressionSyntax True = new JassBooleanLiteralExpressionSyntax(true);
        public static readonly JassBooleanLiteralExpressionSyntax False = new JassBooleanLiteralExpressionSyntax(false);

        private JassBooleanLiteralExpressionSyntax(bool value)
        {
            Value = value;
        }

        public bool Value { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassBooleanLiteralExpressionSyntax booleanLiteralExpression
                && Value == booleanLiteralExpression.Value;
        }

        public override string ToString() => Value ? JassKeyword.True : JassKeyword.False;
    }
}