// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalDeclarationListSyntax.cs" company="Drake53">
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
    public class VJassScopedGlobalDeclarationListSyntax : IEquatable<VJassScopedGlobalDeclarationListSyntax>
    {
        public VJassScopedGlobalDeclarationListSyntax(ImmutableArray<IScopedGlobalDeclarationSyntax> globals)
        {
            Globals = globals;
        }

        public ImmutableArray<IScopedGlobalDeclarationSyntax> Globals { get; }

        public bool Equals(VJassScopedGlobalDeclarationListSyntax? other)
        {
            return other is not null
                && Globals.SequenceEqual(other.Globals);
        }

        public override string ToString() => $"<{base.ToString()}> [{Globals.Length}]";
    }
}