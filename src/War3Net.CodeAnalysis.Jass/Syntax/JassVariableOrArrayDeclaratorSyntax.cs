// ------------------------------------------------------------------------------
// <copyright file="JassVariableOrArrayDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassVariableOrArrayDeclaratorSyntax : JassSyntaxNode
    {
        protected internal override abstract JassVariableOrArrayDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassVariableOrArrayDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}