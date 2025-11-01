// ------------------------------------------------------------------------------
// <copyright file="JassFunctionReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassFunctionReferenceExpressionSyntax : IExpressionSyntax, IJassSyntaxToken
    {
        public JassFunctionReferenceExpressionSyntax(JassIdentifierNameSyntax identifierName)
        {
            IdentifierName = identifierName;
        }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassFunctionReferenceExpressionSyntax functionReferenceExpression
                && IdentifierName.Equals(functionReferenceExpression.IdentifierName);
        }

        public override string ToString() => $"{JassKeyword.Function} {IdentifierName}";
    }
}