// ------------------------------------------------------------------------------
// <copyright file="ArgumentListRenamer.cs" company="Drake53">
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
        private bool TryRenameArgumentList(JassArgumentListSyntax argumentList, [NotNullWhen(true)] out JassArgumentListSyntax? renamedArgumentList)
        {
            var isRenamed = false;

            var argumentsBuilder = ImmutableArray.CreateBuilder<IExpressionSyntax>();
            foreach (var argument in argumentList.Arguments)
            {
                if (TryRenameExpression(argument, out var renamedArgument))
                {
                    argumentsBuilder.Add(renamedArgument);
                    isRenamed = true;
                }
                else
                {
                    argumentsBuilder.Add(argument);
                }
            }

            if (isRenamed)
            {
                renamedArgumentList = new JassArgumentListSyntax(argumentsBuilder.ToImmutable());
                return true;
            }

            renamedArgumentList = null;
            return false;
        }
    }
}