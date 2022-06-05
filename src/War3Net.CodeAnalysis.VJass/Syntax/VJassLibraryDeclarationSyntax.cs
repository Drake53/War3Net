// ------------------------------------------------------------------------------
// <copyright file="VJassLibraryDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLibraryDeclarationSyntax : ITopLevelDeclarationSyntax
    {
        public VJassLibraryDeclarationSyntax(
            VJassIdentifierNameSyntax identifierName,
            VJassIdentifierNameSyntax? initializer,
            VJassLibraryDependencyListSyntax? dependencies,
            VJassScopedDeclarationListSyntax declarations)
        {
            IdentifierName = identifierName;
            Initializer = initializer;
            Dependencies = dependencies;
            Declarations = declarations;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassIdentifierNameSyntax? Initializer { get; }

        public VJassLibraryDependencyListSyntax? Dependencies { get; }

        public VJassScopedDeclarationListSyntax Declarations { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassLibraryDeclarationSyntax libraryDeclaration
                && IdentifierName.Equals(libraryDeclaration.IdentifierName)
                && Initializer.NullableEquals(libraryDeclaration.Initializer)
                && Dependencies.NullableEquals(libraryDeclaration.Dependencies)
                && Declarations.Equals(libraryDeclaration.Declarations);
        }

        public override string ToString() => Dependencies is null
            ? Initializer is null
                ? $"{VJassKeyword.Library} {IdentifierName} [{Declarations.Declarations.Length}]"
                : $"{VJassKeyword.Library} {IdentifierName} {VJassKeyword.Initializer} {Initializer} [{Declarations.Declarations.Length}]"
            : Initializer is null
                ? $"{VJassKeyword.Library} {IdentifierName} {Dependencies} [{Declarations.Declarations.Length}]"
                : $"{VJassKeyword.Library} {IdentifierName} {VJassKeyword.Initializer} {Initializer} {Dependencies} [{Declarations.Declarations.Length}]";
    }
}