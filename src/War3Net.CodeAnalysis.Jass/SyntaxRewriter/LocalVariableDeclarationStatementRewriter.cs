// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationStatementRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="localVariableDeclarationStatement">The <see cref="JassLocalVariableDeclarationStatementSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassStatementSyntax"/>, or the input <paramref name="localVariableDeclarationStatement"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="localVariableDeclarationStatement"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteLocalVariableDeclarationStatement(JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement, out JassStatementSyntax result)
        {
            if (RewriteToken(localVariableDeclarationStatement.LocalToken, out var localToken) |
                RewriteVariableOrArrayDeclarator(localVariableDeclarationStatement.Declarator, out var declarator))
            {
                result = new JassLocalVariableDeclarationStatementSyntax(
                    localToken,
                    declarator);

                return true;
            }

            result = localVariableDeclarationStatement;
            return false;
        }
    }
}