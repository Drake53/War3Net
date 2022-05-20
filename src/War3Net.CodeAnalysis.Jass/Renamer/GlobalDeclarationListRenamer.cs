// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationListRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameGlobalDeclarationList(JassGlobalDeclarationListSyntax globalDeclarationList, [NotNullWhen(true)] out ITopLevelDeclarationSyntax? renamedGlobalDeclarationList)
        {
            var isRenamed = false;

            var declarationsBuilder = ImmutableArray.CreateBuilder<IGlobalDeclarationSyntax>();
            foreach (var declaration in globalDeclarationList.Globals)
            {
                if (TryRenameGlobalDeclaration(declaration, out var renamedDeclaration))
                {
                    declarationsBuilder.Add(renamedDeclaration);
                    isRenamed = true;
                }
                else
                {
                    declarationsBuilder.Add(declaration);
                }
            }

            if (isRenamed)
            {
                renamedGlobalDeclarationList = new JassGlobalDeclarationListSyntax(declarationsBuilder.ToImmutable());
                return true;
            }

            renamedGlobalDeclarationList = null;
            return false;
        }
    }
}