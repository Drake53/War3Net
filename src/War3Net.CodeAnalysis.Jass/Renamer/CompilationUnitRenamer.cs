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

            for (var i = 0; i < compilationUnit.Declarations.Length; i++)
            {
                if (TryRenameDeclaration(compilationUnit.Declarations[i], out var renamedDeclaration))
                {
                    var builder = ImmutableArray.CreateBuilder<JassTopLevelDeclarationSyntax>(compilationUnit.Declarations.Length);
                    for (var j = 0; j < i; j++)
                    {
                        builder.Add(compilationUnit.Declarations[j]);
                    }

                    builder.Add(renamedDeclaration);

                    while (++i < compilationUnit.Declarations.Length)
                    {
                        if (TryRenameDeclaration(compilationUnit.Declarations[i], out renamedDeclaration))
                        {
                            builder.Add(renamedDeclaration);
                        }
                        else
                        {
                            builder.Add(compilationUnit.Declarations[i]);
                        }
                    }

                    renamedCompilationUnit = new JassCompilationUnitSyntax(
                        builder.ToImmutable(),
                        compilationUnit.EndOfFileToken);

                    return true;
                }
            }

            renamedCompilationUnit = null;
            return false;
        }
    }
}