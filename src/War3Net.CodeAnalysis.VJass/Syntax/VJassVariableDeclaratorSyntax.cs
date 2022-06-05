// ------------------------------------------------------------------------------
// <copyright file="VJassVariableDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassVariableDeclaratorSyntax : IVariableDeclaratorSyntax
    {
        public VJassVariableDeclaratorSyntax(
            VJassTypeSyntax type,
            VJassIdentifierNameSyntax identifierName,
            VJassEqualsValueClauseSyntax? value)
        {
            Type = type;
            IdentifierName = identifierName;
            Value = value;
        }

        public VJassTypeSyntax Type { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassEqualsValueClauseSyntax? Value { get; }

        public bool Equals(IVariableDeclaratorSyntax? other)
        {
            return other is VJassVariableDeclaratorSyntax variableDeclarator
                && Type.Equals(variableDeclarator.Type)
                && IdentifierName.Equals(variableDeclarator.IdentifierName)
                && Value.NullableEquals(variableDeclarator.Value);
        }

        public override string ToString() => Value is null
            ? $"{Type} {IdentifierName}"
            : $"{Type} {IdentifierName} {Value}";
    }
}