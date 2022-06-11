// ------------------------------------------------------------------------------
// <copyright file="VJassLibraryDependencyListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLibraryDependencyListSyntax : VJassSyntaxNode
    {
        internal VJassLibraryDependencyListSyntax(
            VJassSyntaxToken requiresToken,
            ImmutableArray<VJassLibraryDependencySyntax> dependencies)
        {
            RequiresToken = requiresToken;
            Dependencies = dependencies;
        }

        public VJassSyntaxToken RequiresToken { get; }

        public ImmutableArray<VJassLibraryDependencySyntax> Dependencies { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassLibraryDependencyListSyntax libraryDependencyList
                && Dependencies.IsEquivalentTo(libraryDependencyList.Dependencies);
        }

        public override void WriteTo(TextWriter writer)
        {
            RequiresToken.WriteTo(writer);
            Dependencies.WriteTo(writer);
        }

        public override string ToString() => $"{RequiresToken} {string.Join(' ', Dependencies)}";

        public override VJassSyntaxToken GetFirstToken() => RequiresToken;

        public override VJassSyntaxToken GetLastToken() => Dependencies[^1].GetLastToken();

        protected internal override VJassLibraryDependencyListSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassLibraryDependencyListSyntax(
                newToken,
                Dependencies);
        }

        protected internal override VJassLibraryDependencyListSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassLibraryDependencyListSyntax(
                RequiresToken,
                Dependencies.ReplaceLastItem(Dependencies[^1].ReplaceLastToken(newToken)));
        }
    }
}