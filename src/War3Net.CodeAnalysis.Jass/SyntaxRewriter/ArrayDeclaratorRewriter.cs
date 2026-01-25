// ------------------------------------------------------------------------------
// <copyright file="ArrayDeclaratorRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="arrayDeclarator">The <see cref="JassArrayDeclaratorSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassVariableOrArrayDeclaratorSyntax"/>, or the input <paramref name="arrayDeclarator"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="arrayDeclarator"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteArrayDeclarator(JassArrayDeclaratorSyntax arrayDeclarator, out JassVariableOrArrayDeclaratorSyntax result)
        {
            if (RewriteType(arrayDeclarator.Type, out var type) |
                RewriteToken(arrayDeclarator.ArrayToken, out var arrayToken) |
                RewriteIdentifierName(arrayDeclarator.IdentifierName, out var identifierName))
            {
                result = new JassArrayDeclaratorSyntax(
                    type,
                    arrayToken,
                    identifierName);

                return true;
            }

            result = arrayDeclarator;
            return false;
        }
    }
}