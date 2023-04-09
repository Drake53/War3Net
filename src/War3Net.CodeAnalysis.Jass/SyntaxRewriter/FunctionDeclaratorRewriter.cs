// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclaratorRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="functionDeclarator">The <see cref="JassFunctionDeclaratorSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassFunctionDeclaratorSyntax"/>, or the input <paramref name="functionDeclarator"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="functionDeclarator"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteFunctionDeclarator(JassFunctionDeclaratorSyntax functionDeclarator, out JassFunctionDeclaratorSyntax result)
        {
            if (RewriteToken(functionDeclarator.ConstantToken, out var constantToken) |
                RewriteToken(functionDeclarator.FunctionToken, out var functionToken) |
                RewriteIdentifierName(functionDeclarator.IdentifierName, out var identifierName) |
                RewriteParameterListOrEmptyParameterList(functionDeclarator.ParameterList, out var parameterList) |
                RewriteReturnClause(functionDeclarator.ReturnClause, out var returnClause))
            {
                result = new JassFunctionDeclaratorSyntax(
                    constantToken,
                    functionToken,
                    identifierName,
                    parameterList,
                    returnClause);

                return true;
            }

            result = functionDeclarator;
            return false;
        }
    }
}