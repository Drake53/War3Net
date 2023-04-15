// ------------------------------------------------------------------------------
// <copyright file="TypeDeclarationNormalizer.cs" company="Drake53">
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
        protected override bool RewriteTypeDeclaration(JassTypeDeclarationSyntax typeDeclaration, out JassTopLevelDeclarationSyntax result)
        {
            _nodes.Add(typeDeclaration);
            var normalized = base.RewriteTypeDeclaration(typeDeclaration, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _requireNewlineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}