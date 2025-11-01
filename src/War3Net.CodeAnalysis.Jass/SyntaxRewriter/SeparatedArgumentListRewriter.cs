// ------------------------------------------------------------------------------
// <copyright file="SeparatedArgumentListRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="argumentList">The <see cref="SeparatedSyntaxList{TItem, TSeparator}"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="SeparatedSyntaxList{TItem, TSeparator}"/>, or the input <paramref name="argumentList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="argumentList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteSeparatedArgumentList(SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken> argumentList, out SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken> result)
        {
            if (argumentList.IsEmpty)
            {
                result = argumentList;
                return false;
            }

            if (RewriteExpression(argumentList.Items[0], out var item))
            {
                var argumentListBuilder = SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.CreateBuilder(item, argumentList.Items.Length);
                for (var i = 1; i < argumentList.Items.Length; i++)
                {
                    RewriteToken(argumentList.Separators[i - 1], out var separator);
                    RewriteExpression(argumentList.Items[i], out item);

                    argumentListBuilder.Add(separator, item);
                }

                result = argumentListBuilder.ToSeparatedSyntaxList();
                return true;
            }

            for (var i = 1; i < argumentList.Items.Length; i++)
            {
                if (RewriteToken(argumentList.Separators[i - 1], out var separator) |
                    RewriteExpression(argumentList.Items[i], out item))
                {
                    var argumentListBuilder = SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.CreateBuilder(argumentList.Items[0], argumentList.Items.Length);
                    for (var j = 1; j < i; j++)
                    {
                        argumentListBuilder.Add(argumentList.Separators[j - 1], argumentList.Items[j]);
                    }

                    argumentListBuilder.Add(separator, item);

                    while (++i < argumentList.Items.Length)
                    {
                        RewriteToken(argumentList.Separators[i - 1], out separator);
                        RewriteExpression(argumentList.Items[i], out item);

                        argumentListBuilder.Add(separator, item);
                    }

                    result = argumentListBuilder.ToSeparatedSyntaxList();
                    return true;
                }
            }

            result = argumentList;
            return false;
        }
    }
}