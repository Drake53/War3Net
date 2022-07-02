// ------------------------------------------------------------------------------
// <copyright file="VJassStructuredTriviaSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassStructuredTriviaSyntax : VJassSyntaxNode, ISyntaxTrivia
    {
        protected internal override abstract VJassStructuredTriviaSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassStructuredTriviaSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}