// ------------------------------------------------------------------------------
// <copyright file="IfClauseNormalizer.cs" company="Drake53">
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
        protected override bool RewriteIfClause(JassIfClauseSyntax ifClause, out JassIfClauseSyntax result)
        {
            _nodes.Add(ifClause);
            var normalized = base.RewriteIfClause(ifClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}