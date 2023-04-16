// ------------------------------------------------------------------------------
// <copyright file="GlobalsDeclarationRenamer.cs" company="Drake53">
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
        private bool TryRenameGlobalsDeclaration(JassGlobalsDeclarationSyntax globalsDeclaration, [NotNullWhen(true)] out JassTopLevelDeclarationSyntax? renamedGlobalsDeclaration)
        {
            for (var i = 0; i < globalsDeclaration.GlobalDeclarations.Length; i++)
            {
                if (TryRenameGlobalDeclaration(globalsDeclaration.GlobalDeclarations[i], out var renamedGlobal))
                {
                    var builder = ImmutableArray.CreateBuilder<JassGlobalDeclarationSyntax>(globalsDeclaration.GlobalDeclarations.Length);
                    for (var j = 0; j < i; j++)
                    {
                        builder.Add(globalsDeclaration.GlobalDeclarations[j]);
                    }

                    builder.Add(renamedGlobal);

                    while (++i < globalsDeclaration.GlobalDeclarations.Length)
                    {
                        if (TryRenameGlobalDeclaration(globalsDeclaration.GlobalDeclarations[i], out renamedGlobal))
                        {
                            builder.Add(renamedGlobal);
                        }
                        else
                        {
                            builder.Add(globalsDeclaration.GlobalDeclarations[i]);
                        }
                    }

                    renamedGlobalsDeclaration = new JassGlobalsDeclarationSyntax(
                        globalsDeclaration.GlobalsToken,
                        builder.ToImmutable(),
                        globalsDeclaration.EndGlobalsToken);

                    return true;
                }
            }

            renamedGlobalsDeclaration = null;
            return false;
        }
    }
}