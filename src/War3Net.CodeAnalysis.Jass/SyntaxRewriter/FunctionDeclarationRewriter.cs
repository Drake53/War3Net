// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="functionDeclaration">The <see cref="JassFunctionDeclarationSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassTopLevelDeclarationSyntax"/>, or the input <paramref name="functionDeclaration"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="functionDeclaration"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteFunctionDeclaration(JassFunctionDeclarationSyntax functionDeclaration, out JassTopLevelDeclarationSyntax result)
        {
            if (RewriteFunctionDeclarator(functionDeclaration.FunctionDeclarator, out var functionDeclarator) |
                RewriteStatementList(functionDeclaration.Statements, out var statements) |
                RewriteToken(functionDeclaration.EndFunctionToken, out var endFunctionToken))
            {
                result = new JassFunctionDeclarationSyntax(
                    functionDeclarator,
                    statements,
                    endFunctionToken);

                return true;
            }

            result = functionDeclaration;
            return false;
        }
    }
}