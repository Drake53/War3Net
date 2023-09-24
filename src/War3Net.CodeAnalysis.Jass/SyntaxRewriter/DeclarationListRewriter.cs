// ------------------------------------------------------------------------------
// <copyright file="DeclarationListRewriter.cs" company="Drake53">
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
        /// <param name="declarationList">The <see cref="ImmutableArray{T}"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="ImmutableArray{T}"/>, or the input <paramref name="declarationList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="declarationList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteDeclarationList(ImmutableArray<JassTopLevelDeclarationSyntax> declarationList, out ImmutableArray<JassTopLevelDeclarationSyntax> result)
        {
            if (declarationList.IsEmpty)
            {
                result = declarationList;
                return false;
            }

            for (var i = 0; i < declarationList.Length; i++)
            {
                if (RewriteTopLevelDeclaration(declarationList[i], out var declaration))
                {
                    var declarationListBuilder = declarationList.ToBuilder();
                    declarationListBuilder[i] = declaration;

                    while (++i < declarationList.Length)
                    {
                        if (RewriteTopLevelDeclaration(declarationList[i], out declaration))
                        {
                            declarationListBuilder[i] = declaration;
                        }
                    }

                    result = declarationListBuilder.ToImmutable();
                    return true;
                }
            }

            result = declarationList;
            return false;
        }
    }
}