// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalDeclarationElseClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopedGlobalDeclarationElseClauseSyntax : IEquatable<VJassScopedGlobalDeclarationElseClauseSyntax>
    {
        public VJassScopedGlobalDeclarationElseClauseSyntax(VJassScopedGlobalDeclarationListSyntax body)
        {
            Body = body;
        }

        public VJassScopedGlobalDeclarationListSyntax Body { get; }

        public bool Equals(VJassScopedGlobalDeclarationElseClauseSyntax? other)
        {
            return other is not null
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.Else} [{Body.Globals.Length}]";
    }
}