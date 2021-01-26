// ------------------------------------------------------------------------------
// <copyright file="ParameterListRenderer.cs" company="Drake53">
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
        public void Render(JassParameterListSyntax parameterList)
        {
            if (parameterList.Parameters.Any())
            {
                Render(parameterList.Parameters.First());
                foreach (var parameter in parameterList.Parameters.Skip(1))
                {
                    Write($"{JassSymbol.Comma} ");
                    Render(parameter);
                }
            }
            else
            {
                Render(JassTypeSyntax.Nothing);
            }
        }
    }
}