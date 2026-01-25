// ------------------------------------------------------------------------------
// <copyright file="JassCompilationUnitSyntaxExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassCompilationUnitSyntaxExtensions
    {
        public static JassCompilationUnitSyntax NormalizeWhitespace(this JassCompilationUnitSyntax compilationUnit)
        {
            return new JassSyntaxNormalizer().NormalizeWhitespace(compilationUnit);
        }
    }
}