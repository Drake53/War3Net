// ------------------------------------------------------------------------------
// <copyright file="VJassModuleImplementationDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassModuleImplementationDeclarationSyntax : IMemberDeclarationSyntax
    {
        public VJassModuleImplementationDeclarationSyntax(
            VJassIdentifierNameSyntax identifierName)
        {
            IdentifierName = identifierName;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public bool Equals(IMemberDeclarationSyntax? other)
        {
            return other is VJassModuleImplementationDeclarationSyntax moduleImplementationDeclaration
                && IdentifierName.Equals(moduleImplementationDeclaration.IdentifierName);
        }

        public override string ToString() => $"{VJassKeyword.Implement} {IdentifierName}";
    }
}