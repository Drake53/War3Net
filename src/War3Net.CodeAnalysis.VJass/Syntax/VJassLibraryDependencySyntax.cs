// ------------------------------------------------------------------------------
// <copyright file="VJassLibraryDependencySyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLibraryDependencySyntax : VJassSyntaxNode
    {
        internal VJassLibraryDependencySyntax(
            VJassSyntaxToken? optionalToken,
            VJassIdentifierNameSyntax identifierName,
            VJassSyntaxToken? commaToken)
        {
            OptionalToken = optionalToken;
            IdentifierName = identifierName;
            CommaToken = commaToken;
        }

        public VJassSyntaxToken? OptionalToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassSyntaxToken? CommaToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassLibraryDependencySyntax libraryDependency
                && OptionalToken.NullableEquals(libraryDependency.OptionalToken)
                && IdentifierName.IsEquivalentTo(libraryDependency.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            OptionalToken?.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            CommaToken?.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            OptionalToken?.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
            CommaToken?.ProcessTo(writer, context);
        }

        public override string ToString() => $"{OptionalToken.OptionalSuffixed()}{IdentifierName}{CommaToken.Optional()}";

        public override VJassSyntaxToken GetFirstToken() => OptionalToken ?? IdentifierName.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => CommaToken ?? IdentifierName.GetLastToken();

        protected internal override VJassLibraryDependencySyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (OptionalToken is not null)
            {
                return new VJassLibraryDependencySyntax(
                    newToken,
                    IdentifierName,
                    CommaToken);
            }

            return new VJassLibraryDependencySyntax(
                null,
                IdentifierName.ReplaceFirstToken(newToken),
                CommaToken);
        }

        protected internal override VJassLibraryDependencySyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (CommaToken is not null)
            {
                return new VJassLibraryDependencySyntax(
                    OptionalToken,
                    IdentifierName,
                    newToken);
            }

            return new VJassLibraryDependencySyntax(
                OptionalToken,
                IdentifierName.ReplaceLastToken(newToken),
                null);
        }
    }
}