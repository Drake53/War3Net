// ------------------------------------------------------------------------------
// <copyright file="JassStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassStatementSyntax : JassSyntaxNode
    {
        protected internal override abstract JassStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassStatementSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}