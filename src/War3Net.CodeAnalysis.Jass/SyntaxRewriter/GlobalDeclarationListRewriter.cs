// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationListRewriter.cs" company="Drake53">
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
        /// <param name="globalDeclarationList">The <see cref="ImmutableArray{T}"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="ImmutableArray{T}"/>, or the input <paramref name="globalDeclarationList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="globalDeclarationList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteGlobalDeclarationList(ImmutableArray<JassGlobalDeclarationSyntax> globalDeclarationList, out ImmutableArray<JassGlobalDeclarationSyntax> result)
        {
            if (globalDeclarationList.IsEmpty)
            {
                result = globalDeclarationList;
                return false;
            }

            for (var i = 0; i < globalDeclarationList.Length; i++)
            {
                if (RewriteGlobalDeclaration(globalDeclarationList[i], out var globalDeclaration))
                {
                    var globalDeclarationListBuilder = ImmutableArray.CreateBuilder<JassGlobalDeclarationSyntax>(globalDeclarationList.Length);
                    globalDeclarationListBuilder.AddRange(globalDeclarationList, i);
                    globalDeclarationListBuilder.Add(globalDeclaration);

                    while (++i < globalDeclarationList.Length)
                    {
                        RewriteGlobalDeclaration(globalDeclarationList[i], out globalDeclaration);
                        globalDeclarationListBuilder.Add(globalDeclaration);
                    }

                    result = globalDeclarationListBuilder.ToImmutable();
                    return true;
                }
            }

            result = globalDeclarationList;
            return false;
        }
    }
}