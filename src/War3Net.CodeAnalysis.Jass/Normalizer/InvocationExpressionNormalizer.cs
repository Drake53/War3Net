// ------------------------------------------------------------------------------
// <copyright file="InvocationExpressionNormalizer.cs" company="Drake53">
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
        protected override bool RewriteInvocationExpression(JassInvocationExpressionSyntax invocationExpression, out JassExpressionSyntax result)
        {
            _nodes.Add(invocationExpression);
            var normalized = base.RewriteInvocationExpression(invocationExpression, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}