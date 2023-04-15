// ------------------------------------------------------------------------------
// <copyright file="IdentifierNameNormalizer.cs" company="Drake53">
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
        protected override bool RewriteIdentifierName(JassIdentifierNameSyntax identifierName, out JassIdentifierNameSyntax result)
        {
            _nodes.Add(identifierName);
            var normalized = base.RewriteIdentifierName(identifierName, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }

        /// <inheritdoc/>
        protected override bool RewriteIdentifierNameAsExpression(JassIdentifierNameSyntax identifierName, out JassExpressionSyntax result)
        {
            _nodes.Add(identifierName);
            var normalized = base.RewriteIdentifierNameAsExpression(identifierName, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }

        /// <inheritdoc/>
        protected override bool RewriteIdentifierNameAsType(JassIdentifierNameSyntax identifierName, out JassTypeSyntax result)
        {
            _nodes.Add(identifierName);
            var normalized = base.RewriteIdentifierNameAsType(identifierName, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}