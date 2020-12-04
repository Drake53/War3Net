// ------------------------------------------------------------------------------
// <copyright file="VariableExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax VariableExpression(string variableName)
        {
            return new NewExpressionSyntax(new ExpressionSyntax(Token(SyntaxTokenType.AlphanumericIdentifier, variableName)), Empty());
        }
    }
}