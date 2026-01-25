// ------------------------------------------------------------------------------
// <copyright file="ReturnClauseNormalizer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteReturnClause(JassReturnClauseSyntax returnClause, out JassReturnClauseSyntax result)
        {
            _nodes.Add(returnClause);
            var normalized = base.RewriteReturnClause(returnClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}