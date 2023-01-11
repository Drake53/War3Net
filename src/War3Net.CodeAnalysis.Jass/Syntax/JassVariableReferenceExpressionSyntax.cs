// ------------------------------------------------------------------------------
// <copyright file="JassVariableReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    [Obsolete]
    public class JassVariableReferenceExpressionSyntax : IExpressionSyntax
    {
        public JassVariableReferenceExpressionSyntax(JassIdentifierNameSyntax identifierName)
        {
            IdentifierName = identifierName;
        }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassVariableReferenceExpressionSyntax variableReferenceExpression
                && IdentifierName.Equals(variableReferenceExpression.IdentifierName);
        }

        public override string ToString() => IdentifierName.ToString();
    }
}