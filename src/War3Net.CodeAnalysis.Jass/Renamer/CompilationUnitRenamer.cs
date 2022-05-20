// ------------------------------------------------------------------------------
// <copyright file="CompilationUnitRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        public bool TryRenameCompilationUnit(JassCompilationUnitSyntax compilationUnit, [NotNullWhen(true)] out JassCompilationUnitSyntax? renamedCompilationUnit)
        {
            if (compilationUnit is null)
            {
                throw new ArgumentNullException(nameof(compilationUnit));
            }

            var isRenamed = false;

            var declarationsBuilder = ImmutableArray.CreateBuilder<ITopLevelDeclarationSyntax>();
            foreach (var declaration in compilationUnit.Declarations)
            {
                if (TryRenameDeclaration(declaration, out var renamedDeclaration))
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
                renamedCompilationUnit = new JassCompilationUnitSyntax(declarationsBuilder.ToImmutable());
                return true;
            }

            renamedCompilationUnit = null;
            return false;
        }
    }
}