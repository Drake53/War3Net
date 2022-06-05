// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalDeclarationListSyntax.cs" company="Drake53">
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
    public class VJassGlobalDeclarationListSyntax : IEquatable<VJassGlobalDeclarationListSyntax>
    {
        public VJassGlobalDeclarationListSyntax(ImmutableArray<IGlobalDeclarationSyntax> globals)
        {
            Globals = globals;
        }

        public ImmutableArray<IGlobalDeclarationSyntax> Globals { get; }

        public bool Equals(VJassGlobalDeclarationListSyntax? other)
        {
            return other is not null
                && Globals.SequenceEqual(other.Globals);
        }

        public override string ToString() => $"<{base.ToString()}> [{Globals.Length}]";
    }
}