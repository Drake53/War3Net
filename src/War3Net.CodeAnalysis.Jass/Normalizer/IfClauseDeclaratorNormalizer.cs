// ------------------------------------------------------------------------------
// <copyright file="IfClauseDeclaratorNormalizer.cs" company="Drake53">
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
        protected override bool RewriteIfClauseDeclarator(JassIfClauseDeclaratorSyntax ifClauseDeclarator, out JassIfClauseDeclaratorSyntax result)
        {
            _nodes.Add(ifClauseDeclarator);
            var normalized = base.RewriteIfClauseDeclarator(ifClauseDeclarator, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}