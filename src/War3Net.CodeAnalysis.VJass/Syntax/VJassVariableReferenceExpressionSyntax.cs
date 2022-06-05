// ------------------------------------------------------------------------------
// <copyright file="VJassVariableReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassVariableReferenceExpressionSyntax : IExpressionSyntax
    {
        public VJassVariableReferenceExpressionSyntax(VJassIdentifierNameSyntax identifierName)
        {
            IdentifierName = identifierName;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is VJassVariableReferenceExpressionSyntax variableReferenceExpression
                && IdentifierName.Equals(variableReferenceExpression.IdentifierName);
        }

        public override string ToString() => IdentifierName.ToString();
    }
}