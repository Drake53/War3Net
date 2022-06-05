// ------------------------------------------------------------------------------
// <copyright file="VJassMemberDeclarationElseClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMemberDeclarationElseClauseSyntax : IEquatable<VJassMemberDeclarationElseClauseSyntax>
    {
        public VJassMemberDeclarationElseClauseSyntax(VJassMemberDeclarationListSyntax body)
        {
            Body = body;
        }

        public VJassMemberDeclarationListSyntax Body { get; }

        public bool Equals(VJassMemberDeclarationElseClauseSyntax? other)
        {
            return other is not null
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.Else} [{Body.Declarations.Length}]";
    }
}