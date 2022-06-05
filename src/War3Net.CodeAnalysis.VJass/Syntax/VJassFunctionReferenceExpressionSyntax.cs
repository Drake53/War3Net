// ------------------------------------------------------------------------------
// <copyright file="VJassFunctionReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassFunctionReferenceExpressionSyntax : IExpressionSyntax
    {
        public VJassFunctionReferenceExpressionSyntax(
            VJassIdentifierNameSyntax identifierName)
        {
            IdentifierName = identifierName;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is VJassFunctionReferenceExpressionSyntax functionReferenceExpression
                && IdentifierName.Equals(functionReferenceExpression.IdentifierName);
        }

        public override string ToString() => $"{VJassKeyword.Function} {IdentifierName}";
    }
}