// ------------------------------------------------------------------------------
// <copyright file="VJassScopedDeclarationElseClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopedDeclarationElseClauseSyntax : IEquatable<VJassScopedDeclarationElseClauseSyntax>
    {
        public VJassScopedDeclarationElseClauseSyntax(VJassScopedDeclarationListSyntax body)
        {
            Body = body;
        }

        public VJassScopedDeclarationListSyntax Body { get; }

        public bool Equals(VJassScopedDeclarationElseClauseSyntax? other)
        {
            return other is not null
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.Else} [{Body.Declarations.Length}]";
    }
}