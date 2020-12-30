// ------------------------------------------------------------------------------
// <copyright file="JassFunctionReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassFunctionReferenceExpressionSyntax : IExpressionSyntax
    {
        public JassFunctionReferenceExpressionSyntax(JassIdentifierNameSyntax identifierName)
        {
            IdentifierName = identifierName;
        }

        public JassFunctionReferenceExpressionSyntax(string name)
        {
            IdentifierName = new JassIdentifierNameSyntax(name);
        }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public bool Equals(IExpressionSyntax? other) => other is JassFunctionReferenceExpressionSyntax e && IdentifierName.Equals(e.IdentifierName);

        public override string ToString() => $"function {IdentifierName}";
    }
}