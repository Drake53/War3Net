// ------------------------------------------------------------------------------
// <copyright file="VJassTopLevelDeclarationElseClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassTopLevelDeclarationElseClauseSyntax : IEquatable<VJassTopLevelDeclarationElseClauseSyntax>
    {
        public VJassTopLevelDeclarationElseClauseSyntax(
            VJassTopLevelDeclarationListSyntax body)
        {
            Body = body;
        }

        public VJassTopLevelDeclarationListSyntax Body { get; }

        public bool Equals(VJassTopLevelDeclarationElseClauseSyntax? other)
        {
            return other is not null
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.Else} [{Body.Declarations.Length}]";
    }
}