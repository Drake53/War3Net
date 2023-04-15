// ------------------------------------------------------------------------------
// <copyright file="VariableDeclaratorNormalizer.cs" company="Drake53">
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
        protected override bool RewriteVariableDeclarator(JassVariableDeclaratorSyntax variableDeclarator, out JassVariableOrArrayDeclaratorSyntax result)
        {
            _nodes.Add(variableDeclarator);
            var normalized = base.RewriteVariableDeclarator(variableDeclarator, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}