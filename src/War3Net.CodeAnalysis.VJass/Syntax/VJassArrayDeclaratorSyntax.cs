// ------------------------------------------------------------------------------
// <copyright file="VJassArrayDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassArrayDeclaratorSyntax : IVariableDeclaratorSyntax
    {
        public VJassArrayDeclaratorSyntax(
            VJassTypeSyntax type,
            VJassIdentifierNameSyntax identifierName)
        {
            Type = type;
            IdentifierName = identifierName;
        }

        public VJassTypeSyntax Type { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public bool Equals(IVariableDeclaratorSyntax? other)
        {
            return other is VJassArrayDeclaratorSyntax arrayDeclarator
                && Type.Equals(arrayDeclarator.Type)
                && IdentifierName.Equals(arrayDeclarator.IdentifierName);
        }

        public override string ToString() => $"{Type} {VJassKeyword.Array} {IdentifierName}";
    }
}