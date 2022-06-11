// ------------------------------------------------------------------------------
// <copyright file="VJassLibraryDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLibraryDeclaratorSyntax : VJassSyntaxNode
    {
        internal VJassLibraryDeclaratorSyntax(
            VJassSyntaxToken libraryToken,
            VJassIdentifierNameSyntax identifierName,
            VJassInitializerSyntax? initializer,
            VJassLibraryDependencyListSyntax? dependencies)
        {
            LibraryToken = libraryToken;
            IdentifierName = identifierName;
            Initializer = initializer;
            Dependencies = dependencies;
        }

        public VJassSyntaxToken LibraryToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassInitializerSyntax? Initializer { get; }

        public VJassLibraryDependencyListSyntax? Dependencies { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassLibraryDeclaratorSyntax libraryDeclarator
                && IdentifierName.IsEquivalentTo(libraryDeclarator.IdentifierName)
                && Initializer.NullableEquals(libraryDeclarator.Initializer)
                && Dependencies.NullableEquals(libraryDeclarator.Dependencies);
        }

        public override void WriteTo(TextWriter writer)
        {
            LibraryToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            Initializer?.WriteTo(writer);
            Dependencies?.WriteTo(writer);
        }

        public override string ToString() => $"{LibraryToken} {IdentifierName}{Initializer.OptionalPrefixed()}{Dependencies.OptionalPrefixed()}";

        public override VJassSyntaxToken GetFirstToken() => LibraryToken;

        public override VJassSyntaxToken GetLastToken() => (((VJassSyntaxNode?)Dependencies ?? Initializer) ?? IdentifierName).GetLastToken();

        protected internal override VJassLibraryDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassLibraryDeclaratorSyntax(
                newToken,
                IdentifierName,
                Initializer,
                Dependencies);
        }

        protected internal override VJassLibraryDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (Dependencies is not null)
            {
                return new VJassLibraryDeclaratorSyntax(
                    LibraryToken,
                    IdentifierName,
                    Initializer,
                    Dependencies.ReplaceLastToken(newToken));
            }

            if (Initializer is not null)
            {
                return new VJassLibraryDeclaratorSyntax(
                    LibraryToken,
                    IdentifierName,
                    Initializer.ReplaceLastToken(newToken),
                    null);
            }

            return new VJassLibraryDeclaratorSyntax(
                LibraryToken,
                IdentifierName.ReplaceLastToken(newToken),
                null,
                null);
        }
    }
}