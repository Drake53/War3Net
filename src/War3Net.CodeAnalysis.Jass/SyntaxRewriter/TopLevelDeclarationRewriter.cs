// ------------------------------------------------------------------------------
// <copyright file="TopLevelDeclarationRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="topLevelDeclaration">The <see cref="JassTopLevelDeclarationSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassTopLevelDeclarationSyntax"/>, or the input <paramref name="topLevelDeclaration"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="topLevelDeclaration"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteTopLevelDeclaration(JassTopLevelDeclarationSyntax topLevelDeclaration, out JassTopLevelDeclarationSyntax result)
        {
            return topLevelDeclaration switch
            {
                JassFunctionDeclarationSyntax functionDeclaration => RewriteFunctionDeclaration(functionDeclaration, out result),
                JassGlobalsDeclarationSyntax globalsDeclaration => RewriteGlobalsDeclaration(globalsDeclaration, out result),
                JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration => RewriteNativeFunctionDeclaration(nativeFunctionDeclaration, out result),
                JassTypeDeclarationSyntax typeDeclaration => RewriteTypeDeclaration(typeDeclaration, out result),
            };
        }
    }
}