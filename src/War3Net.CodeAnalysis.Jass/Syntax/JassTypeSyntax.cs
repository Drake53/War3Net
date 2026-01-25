// ------------------------------------------------------------------------------
// <copyright file="JassTypeSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassTypeSyntax : JassExpressionSyntax
    {
        protected internal override abstract JassTypeSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassTypeSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}