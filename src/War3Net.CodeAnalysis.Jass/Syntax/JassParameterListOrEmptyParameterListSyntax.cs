// ------------------------------------------------------------------------------
// <copyright file="JassParameterListOrEmptyParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassParameterListOrEmptyParameterListSyntax : JassSyntaxNode
    {
        public abstract JassSyntaxToken TakesToken { get; }

        public abstract ImmutableArray<JassParameterSyntax> Parameters { get; }

        protected internal override abstract JassParameterListOrEmptyParameterListSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassParameterListOrEmptyParameterListSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}