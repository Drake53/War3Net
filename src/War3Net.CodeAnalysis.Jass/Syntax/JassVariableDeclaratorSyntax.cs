// ------------------------------------------------------------------------------
// <copyright file="JassVariableDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassVariableDeclaratorSyntax : IVariableDeclaratorSyntax, IJassSyntaxToken
    {
        public JassVariableDeclaratorSyntax(JassTypeSyntax type, JassIdentifierNameSyntax identifierName, JassEqualsValueClauseSyntax? value)
        {
            Type = type;
            IdentifierName = identifierName;
            Value = value;
        }

        public JassTypeSyntax Type { get; init; }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public JassEqualsValueClauseSyntax? Value { get; init; }

        public bool Equals(IVariableDeclaratorSyntax? other)
        {
            return other is JassVariableDeclaratorSyntax variableDeclarator
                && Type.Equals(variableDeclarator.Type)
                && IdentifierName.Equals(variableDeclarator.IdentifierName)
                && Value.NullableEquals(variableDeclarator.Value);
        }

        public override string ToString()
        {
            return Value is null
                ? $"{Type} {IdentifierName}"
                : $"{Type} {IdentifierName} {Value}";
        }
    }
}