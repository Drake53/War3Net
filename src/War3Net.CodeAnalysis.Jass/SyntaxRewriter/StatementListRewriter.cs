// ------------------------------------------------------------------------------
// <copyright file="StatementListRewriter.cs" company="Drake53">
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
        /// <param name="statementList">The <see cref="ImmutableArray{T}"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="ImmutableArray{T}"/>, or the input <paramref name="statementList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="statementList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteStatementList(ImmutableArray<JassStatementSyntax> statementList, out ImmutableArray<JassStatementSyntax> result)
        {
            if (statementList.IsEmpty)
            {
                result = statementList;
                return false;
            }

            for (var i = 0; i < statementList.Length; i++)
            {
                if (RewriteStatement(statementList[i], out var statement))
                {
                    var statementListBuilder = ImmutableArray.CreateBuilder<JassStatementSyntax>(statementList.Length);
                    statementListBuilder.AddRange(statementList, i);
                    statementListBuilder.Add(statement);

                    while (++i < statementList.Length)
                    {
                        RewriteStatement(statementList[i], out statement);
                        statementListBuilder.Add(statement);
                    }

                    result = statementListBuilder.ToImmutable();
                    return true;
                }
            }

            result = statementList;
            return false;
        }
    }
}