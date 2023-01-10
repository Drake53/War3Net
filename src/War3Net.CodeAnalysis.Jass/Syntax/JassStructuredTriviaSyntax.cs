// ------------------------------------------------------------------------------
// <copyright file="JassStructuredTriviaSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassStructuredTriviaSyntax : JassSyntaxNode, ISyntaxTrivia
    {
        protected internal override abstract JassStructuredTriviaSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassStructuredTriviaSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}