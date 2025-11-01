// ------------------------------------------------------------------------------
// <copyright file="ParameterRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="parameter">The <see cref="JassParameterSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassParameterSyntax"/>, or the input <paramref name="parameter"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="parameter"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteParameter(JassParameterSyntax parameter, out JassParameterSyntax result)
        {
            if (RewriteType(parameter.Type, out var type) |
                RewriteIdentifierName(parameter.IdentifierName, out var identifierName))
            {
                result = new JassParameterSyntax(
                    type,
                    identifierName);

                return true;
            }

            result = parameter;
            return false;
        }
    }
}