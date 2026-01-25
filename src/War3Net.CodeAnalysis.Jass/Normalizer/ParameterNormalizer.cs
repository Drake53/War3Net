// ------------------------------------------------------------------------------
// <copyright file="ParameterNormalizer.cs" company="Drake53">
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
        protected override bool RewriteParameter(JassParameterSyntax parameter, out JassParameterSyntax result)
        {
            _nodes.Add(parameter);
            var normalized = base.RewriteParameter(parameter, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}