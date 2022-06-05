// ------------------------------------------------------------------------------
// <copyright file="VJassCompilationUnitSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassCompilationUnitSyntax : IEquatable<VJassCompilationUnitSyntax>
    {
        public VJassCompilationUnitSyntax(ImmutableArray<ITopLevelDeclarationSyntax> declarations)
        {
            Declarations = declarations;
        }

        public ImmutableArray<ITopLevelDeclarationSyntax> Declarations { get; }

        public bool Equals(VJassCompilationUnitSyntax? other)
        {
            return other is not null
                && Declarations.SequenceEqual(other.Declarations);
        }

        public override string ToString() => $"<{base.ToString()}> [{Declarations.Length}]";
    }
}