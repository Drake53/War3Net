// ------------------------------------------------------------------------------
// <copyright file="CompilationUnitRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="compilationUnit">The <see cref="JassCompilationUnitSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassCompilationUnitSyntax"/>, or the input <paramref name="compilationUnit"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="compilationUnit"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteCompilationUnit(JassCompilationUnitSyntax compilationUnit, out JassCompilationUnitSyntax result)
        {
            if (RewriteDeclarationList(compilationUnit.Declarations, out var declarations) |
                RewriteToken(compilationUnit.EndOfFileToken, out var endOfFileToken))
            {
                result = new JassCompilationUnitSyntax(
                    declarations,
                    endOfFileToken);

                return true;
            }

            result = compilationUnit;
            return false;
        }
    }
}