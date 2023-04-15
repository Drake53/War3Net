// ------------------------------------------------------------------------------
// <copyright file="CompilationUnitNormalizer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        public JassCompilationUnitSyntax NormalizeWhitespace(JassCompilationUnitSyntax compilationUnit)
        {
            RewriteCompilationUnit(compilationUnit, out var result);
            return result;
        }

        /// <inheritdoc/>
        protected override bool RewriteCompilationUnit(JassCompilationUnitSyntax compilationUnit, out JassCompilationUnitSyntax result)
        {
            _nodes.Add(compilationUnit);
            var normalized = base.RewriteCompilationUnit(compilationUnit, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}