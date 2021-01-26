// ------------------------------------------------------------------------------
// <copyright file="ArgumentListRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassArgumentListSyntax argumentList)
        {
            if (argumentList.Arguments.Any())
            {
                Render(argumentList.Arguments.First());
                foreach (var argument in argumentList.Arguments.Skip(1))
                {
                    Write($"{JassSymbol.Comma} ");
                    Render(argument);
                }
            }
        }
    }
}