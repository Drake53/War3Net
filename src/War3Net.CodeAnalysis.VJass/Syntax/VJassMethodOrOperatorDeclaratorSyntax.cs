// ------------------------------------------------------------------------------
// <copyright file="VJassMethodOrOperatorDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassMethodOrOperatorDeclaratorSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassMethodOrOperatorDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassMethodOrOperatorDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}