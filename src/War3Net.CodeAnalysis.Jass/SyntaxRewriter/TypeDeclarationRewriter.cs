// ------------------------------------------------------------------------------
// <copyright file="TypeDeclarationRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="typeDeclaration">The <see cref="JassTypeDeclarationSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassTopLevelDeclarationSyntax"/>, or the input <paramref name="typeDeclaration"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="typeDeclaration"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteTypeDeclaration(JassTypeDeclarationSyntax typeDeclaration, out JassTopLevelDeclarationSyntax result)
        {
            if (RewriteToken(typeDeclaration.TypeToken, out var typeToken) |
                RewriteIdentifierName(typeDeclaration.IdentifierName, out var identifierName) |
                RewriteToken(typeDeclaration.ExtendsToken, out var extendsToken) |
                RewriteType(typeDeclaration.BaseType, out var baseType))
            {
                result = new JassTypeDeclarationSyntax(
                    typeToken,
                    identifierName,
                    extendsToken,
                    baseType);

                return true;
            }

            result = typeDeclaration;
            return false;
        }
    }
}