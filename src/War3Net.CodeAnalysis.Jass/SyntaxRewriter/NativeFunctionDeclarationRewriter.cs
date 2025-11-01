// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="nativeFunctionDeclaration">The <see cref="JassNativeFunctionDeclarationSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassTopLevelDeclarationSyntax"/>, or the input <paramref name="nativeFunctionDeclaration"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="nativeFunctionDeclaration"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteNativeFunctionDeclaration(JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration, out JassTopLevelDeclarationSyntax result)
        {
            if (RewriteToken(nativeFunctionDeclaration.ConstantToken, out var constantToken) |
                RewriteToken(nativeFunctionDeclaration.NativeToken, out var nativeToken) |
                RewriteIdentifierName(nativeFunctionDeclaration.IdentifierName, out var identifierName) |
                RewriteParameterListOrEmptyParameterList(nativeFunctionDeclaration.ParameterList, out var parameterList) |
                RewriteReturnClause(nativeFunctionDeclaration.ReturnClause, out var returnClause))
            {
                result = new JassNativeFunctionDeclarationSyntax(
                    constantToken,
                    nativeToken,
                    identifierName,
                    parameterList,
                    returnClause);

                return true;
            }

            result = nativeFunctionDeclaration;
            return false;
        }
    }
}