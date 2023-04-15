// ------------------------------------------------------------------------------
// <copyright file="EqualsValueClauseNormalizer.cs" company="Drake53">
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
        protected override bool RewriteEqualsValueClause(JassEqualsValueClauseSyntax? equalsValueClause, [NotNullIfNotNull("equalsValueClause")] out JassEqualsValueClauseSyntax? result)
        {
            if (equalsValueClause is null)
            {
                result = null;
                return false;
            }

            _nodes.Add(equalsValueClause);
            var normalized = base.RewriteEqualsValueClause(equalsValueClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}