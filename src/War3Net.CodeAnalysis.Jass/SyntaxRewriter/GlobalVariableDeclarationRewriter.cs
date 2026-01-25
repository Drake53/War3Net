// ------------------------------------------------------------------------------
// <copyright file="GlobalVariableDeclarationRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="globalVariableDeclaration">The <see cref="JassGlobalVariableDeclarationSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassGlobalDeclarationSyntax"/>, or the input <paramref name="globalVariableDeclaration"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="globalVariableDeclaration"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteGlobalVariableDeclaration(JassGlobalVariableDeclarationSyntax globalVariableDeclaration, out JassGlobalDeclarationSyntax result)
        {
            if (RewriteVariableOrArrayDeclarator(globalVariableDeclaration.Declarator, out var declarator))
            {
                result = new JassGlobalVariableDeclarationSyntax(declarator);
                return true;
            }

            result = globalVariableDeclaration;
            return false;
        }
    }
}