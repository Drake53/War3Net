// ------------------------------------------------------------------------------
// <copyright file="VJassStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassStatementSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}