// ------------------------------------------------------------------------------
// <copyright file="TypeRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="type">The <see cref="JassTypeSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassTypeSyntax"/>, or the input <paramref name="type"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="type"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteType(JassTypeSyntax type, out JassTypeSyntax result)
        {
            if (RewriteIdentifierName(type.TypeName, out var typeName))
            {
                result = new JassTypeSyntax(typeName);
                return true;
            }

            result = type;
            return false;
        }
    }
}