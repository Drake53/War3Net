// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax FunctionReferenceExpression(string functionName)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(new FunctionReferenceSyntax(
                    Token(SyntaxTokenType.FunctionKeyword),
                    Token(SyntaxTokenType.AlphanumericIdentifier, functionName))),
                Empty());
        }
    }
}