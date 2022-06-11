// ------------------------------------------------------------------------------
// <copyright file="VJassParameterListOrEmptyParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassParameterListOrEmptyParameterListSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassParameterListOrEmptyParameterListSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassParameterListOrEmptyParameterListSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}