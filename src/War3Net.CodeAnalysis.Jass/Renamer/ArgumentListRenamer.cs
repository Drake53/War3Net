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
            for (var i = 0; i < argumentList.ArgumentList.Items.Length; i++)
            {
                if (TryRenameExpression(argumentList.ArgumentList.Items[i], out var renamedArgument))
                {
                    SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.Builder argumentListBuilder;
                    if (i == 0)
                    {
                        argumentListBuilder = SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.CreateBuilder(renamedArgument, argumentList.ArgumentList.Items.Length);
                    }
                    else
                    {
                        argumentListBuilder = SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.CreateBuilder(argumentList.ArgumentList.Items[0], argumentList.ArgumentList.Items.Length);
                        for (var j = 0; j < i; j++)
                        {
                            argumentListBuilder.Add(argumentList.ArgumentList.Separators[j], argumentList.ArgumentList.Items[j + 1]);
                        }

                        argumentListBuilder.Add(argumentList.ArgumentList.Separators[i - 1], renamedArgument);
                    }

                    while (++i < argumentList.ArgumentList.Items.Length)
                    {
                        if (TryRenameExpression(argumentList.ArgumentList.Items[i], out renamedArgument))
                        {
                            argumentListBuilder.Add(argumentList.ArgumentList.Separators[i - 1], renamedArgument);
                        }
                        else
                        {
                            argumentListBuilder.Add(argumentList.ArgumentList.Separators[i - 1], argumentList.ArgumentList.Items[i]);
                        }
                    }

                    renamedArgumentList = new JassArgumentListSyntax(
                        argumentList.OpenParenToken,
                        argumentListBuilder.ToSeparatedSyntaxList(),
                        argumentList.CloseParenToken);

                    return true;
                }
            }

            renamedArgumentList = null;
            return false;
        }
    }
}