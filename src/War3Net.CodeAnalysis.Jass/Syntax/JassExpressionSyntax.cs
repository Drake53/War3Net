// ------------------------------------------------------------------------------
// <copyright file="JassExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassExpressionSyntax : JassSyntaxNode
    {
        protected internal override abstract JassExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}