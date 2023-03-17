// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassFunctionReferenceExpressionSyntax FunctionReferenceExpression(JassIdentifierNameSyntax identifierName)
        {
            return new JassFunctionReferenceExpressionSyntax(
                Token(JassSyntaxKind.FunctionKeyword),
                identifierName);
        }

        public static JassFunctionReferenceExpressionSyntax FunctionReferenceExpression(string name)
        {
            return new JassFunctionReferenceExpressionSyntax(
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name));
        }
    }
}