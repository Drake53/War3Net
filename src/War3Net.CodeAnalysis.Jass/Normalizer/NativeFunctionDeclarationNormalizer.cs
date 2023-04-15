// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationNormalizer.cs" company="Drake53">
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
        protected override bool RewriteNativeFunctionDeclaration(JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration, out JassTopLevelDeclarationSyntax result)
        {
            _nodes.Add(nativeFunctionDeclaration);
            var normalized = base.RewriteNativeFunctionDeclaration(nativeFunctionDeclaration, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _requireNewlineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}