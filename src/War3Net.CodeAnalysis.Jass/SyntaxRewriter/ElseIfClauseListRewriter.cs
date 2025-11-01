// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseListRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="elseIfClauseList">The <see cref="ImmutableArray{T}"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="ImmutableArray{T}"/>, or the input <paramref name="elseIfClauseList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="elseIfClauseList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteElseIfClauseList(ImmutableArray<JassElseIfClauseSyntax> elseIfClauseList, out ImmutableArray<JassElseIfClauseSyntax> result)
        {
            if (elseIfClauseList.IsEmpty)
            {
                result = elseIfClauseList;
                return false;
            }

            for (var i = 0; i < elseIfClauseList.Length; i++)
            {
                if (RewriteElseIfClause(elseIfClauseList[i], out var elseIfClause))
                {
                    var elseIfClauseListBuilder = ImmutableArray.CreateBuilder<JassElseIfClauseSyntax>(elseIfClauseList.Length);
                    elseIfClauseListBuilder.AddRange(elseIfClauseList, i);
                    elseIfClauseListBuilder.Add(elseIfClause);

                    while (++i < elseIfClauseList.Length)
                    {
                        RewriteElseIfClause(elseIfClauseList[i], out elseIfClause);
                        elseIfClauseListBuilder.Add(elseIfClause);
                    }

                    result = elseIfClauseListBuilder.ToImmutable();
                    return true;
                }
            }

            result = elseIfClauseList;
            return false;
        }
    }
}