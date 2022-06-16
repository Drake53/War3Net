// ------------------------------------------------------------------------------
// <copyright file="VJassVariableOrArrayDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassVariableOrArrayDeclaratorSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassVariableOrArrayDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassVariableOrArrayDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}