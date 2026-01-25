// ------------------------------------------------------------------------------
// <copyright file="ElseClauseNormalizer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteElseClause(JassElseClauseSyntax? elseClause, [NotNullIfNotNull("elseClause")] out JassElseClauseSyntax? result)
        {
            if (elseClause is null)
            {
                result = null;
                return false;
            }

            _nodes.Add(elseClause);
            var normalized = base.RewriteElseClause(elseClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}