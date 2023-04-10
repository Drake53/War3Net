// ------------------------------------------------------------------------------
// <copyright file="PredefinedTypeRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="predefinedType">The <see cref="JassPredefinedTypeSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassTypeSyntax"/>, or the input <paramref name="predefinedType"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="predefinedType"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewritePredefinedType(JassPredefinedTypeSyntax predefinedType, out JassTypeSyntax result)
        {
            if (RewriteToken(predefinedType.Token, out var token))
            {
                result = new JassPredefinedTypeSyntax(token);
                return true;
            }

            result = predefinedType;
            return false;
        }
    }
}