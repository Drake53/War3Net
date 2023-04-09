// ------------------------------------------------------------------------------
// <copyright file="GlobalsDeclarationRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="globalsDeclaration">The <see cref="JassGlobalsDeclarationSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassTopLevelDeclarationSyntax"/>, or the input <paramref name="globalsDeclaration"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="globalsDeclaration"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteGlobalsDeclaration(JassGlobalsDeclarationSyntax globalsDeclaration, out JassTopLevelDeclarationSyntax result)
        {
            if (RewriteToken(globalsDeclaration.GlobalsToken, out var globalsToken) |
                RewriteGlobalDeclarationList(globalsDeclaration.Globals, out var globals) |
                RewriteToken(globalsDeclaration.EndGlobalsToken, out var endGlobalsToken))
            {
                result = new JassGlobalsDeclarationSyntax(
                    globalsToken,
                    globals,
                    endGlobalsToken);

                return true;
            }

            result = globalsDeclaration;
            return false;
        }
    }
}