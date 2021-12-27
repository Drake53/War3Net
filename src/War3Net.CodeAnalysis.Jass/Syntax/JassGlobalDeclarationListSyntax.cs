﻿// ------------------------------------------------------------------------------
// <copyright file="JassGlobalDeclarationListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassGlobalDeclarationListSyntax : IDeclarationSyntax
    {
        public JassGlobalDeclarationListSyntax(ImmutableArray<IDeclarationSyntax> globals)
        {
            Globals = globals;
        }

        public ImmutableArray<IDeclarationSyntax> Globals { get; init; }

        public bool Equals(IDeclarationSyntax? other)
        {
            return other is JassGlobalDeclarationListSyntax globalDeclarationList
                && Globals.SequenceEqual(globalDeclarationList.Globals);
        }

        public override string ToString() => $"{JassKeyword.Globals} [{Globals.Length}]";
    }
}