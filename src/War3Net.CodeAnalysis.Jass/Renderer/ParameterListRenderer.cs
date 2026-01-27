// ------------------------------------------------------------------------------
// <copyright file="ParameterListRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassParameterListSyntax parameterList)
        {
            Render(parameterList.TakesToken);
            WriteSpace();

            Render(parameterList.Parameters.Items[0]);
            for (var i = 1; i < parameterList.Parameters.Items.Length; i++)
            {
                Render(parameterList.Parameters.Separators[i - 1]);
                WriteSpace();
                Render(parameterList.Parameters.Items[i]);
            }
        }
    }
}