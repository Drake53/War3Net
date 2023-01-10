// ------------------------------------------------------------------------------
// <copyright file="JassParameterListOrEmptyParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassParameterListOrEmptyParameterListSyntax : JassSyntaxNode
    {
        protected internal override abstract JassParameterListOrEmptyParameterListSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassParameterListOrEmptyParameterListSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}