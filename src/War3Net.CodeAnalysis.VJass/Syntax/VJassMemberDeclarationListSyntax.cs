﻿// ------------------------------------------------------------------------------
// <copyright file="VJassMemberDeclarationListSyntax.cs" company="Drake53">
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
    public class VJassMemberDeclarationListSyntax : IEquatable<VJassMemberDeclarationListSyntax>
    {
        public VJassMemberDeclarationListSyntax(ImmutableArray<IMemberDeclarationSyntax> declarations)
        {
            Declarations = declarations;
        }

        public ImmutableArray<IMemberDeclarationSyntax> Declarations { get; }

        public bool Equals(VJassMemberDeclarationListSyntax? other)
        {
            return other is not null
                && Declarations.SequenceEqual(other.Declarations);
        }

        public override string ToString() => $"<{base.ToString()}> [{Declarations.Length}]";
    }
}