// ------------------------------------------------------------------------------
// <copyright file="SeparatedParameterListRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="parameterList">The <see cref="SeparatedSyntaxList{TItem, TSeparator}"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="SeparatedSyntaxList{TItem, TSeparator}"/>, or the input <paramref name="parameterList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="parameterList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteSeparatedParameterList(SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> parameterList, out SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> result)
        {
            if (parameterList.IsEmpty)
            {
                result = parameterList;
                return false;
            }

            if (RewriteParameter(parameterList.Items[0], out var item))
            {
                var parameterListBuilder = SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken>.CreateBuilder(item, parameterList.Items.Length);
                for (var i = 1; i < parameterList.Items.Length; i++)
                {
                    RewriteToken(parameterList.Separators[i - 1], out var separator);
                    RewriteParameter(parameterList.Items[i], out item);

                    parameterListBuilder.Add(separator, item);
                }

                result = parameterListBuilder.ToSeparatedSyntaxList();
                return true;
            }

            for (var i = 1; i < parameterList.Items.Length; i++)
            {
                if (RewriteToken(parameterList.Separators[i - 1], out var separator) |
                    RewriteParameter(parameterList.Items[i], out item))
                {
                    var parameterListBuilder = SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken>.CreateBuilder(parameterList.Items[0], parameterList.Items.Length);
                    for (var j = 1; j < i; j++)
                    {
                        parameterListBuilder.Add(parameterList.Separators[j - 1], parameterList.Items[j]);
                    }

                    parameterListBuilder.Add(separator, item);

                    while (++i < parameterList.Items.Length)
                    {
                        RewriteToken(parameterList.Separators[i - 1], out separator);
                        RewriteParameter(parameterList.Items[i], out item);

                        parameterListBuilder.Add(separator, item);
                    }

                    result = parameterListBuilder.ToSeparatedSyntaxList();
                    return true;
                }
            }

            result = parameterList;
            return false;
        }
    }
}