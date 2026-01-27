// ------------------------------------------------------------------------------
// <copyright file="ArgumentListRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameArgumentList(JassArgumentListSyntax argumentList, [NotNullWhen(true)] out JassArgumentListSyntax? renamedArgumentList)
        {
            for (var i = 0; i < argumentList.Arguments.Items.Length; i++)
            {
                if (TryRenameExpression(argumentList.Arguments.Items[i], out var renamedArgument))
                {
                    SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.Builder argumentsBuilder;
                    if (i == 0)
                    {
                        argumentsBuilder = SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.CreateBuilder(renamedArgument, argumentList.Arguments.Items.Length);
                    }
                    else
                    {
                        argumentsBuilder = SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.CreateBuilder(argumentList.Arguments.Items[0], argumentList.Arguments.Items.Length);
                        for (var j = 0; j < i; j++)
                        {
                            argumentsBuilder.Add(argumentList.Arguments.Separators[j], argumentList.Arguments.Items[j + 1]);
                        }

                        argumentsBuilder.Add(argumentList.Arguments.Separators[i - 1], renamedArgument);
                    }

                    while (++i < argumentList.Arguments.Items.Length)
                    {
                        if (TryRenameExpression(argumentList.Arguments.Items[i], out renamedArgument))
                        {
                            argumentsBuilder.Add(argumentList.Arguments.Separators[i - 1], renamedArgument);
                        }
                        else
                        {
                            argumentsBuilder.Add(argumentList.Arguments.Separators[i - 1], argumentList.Arguments.Items[i]);
                        }
                    }

                    renamedArgumentList = new JassArgumentListSyntax(
                        argumentList.OpenParenToken,
                        argumentsBuilder.ToSeparatedSyntaxList(),
                        argumentList.CloseParenToken);

                    return true;
                }
            }

            renamedArgumentList = null;
            return false;
        }
    }
}