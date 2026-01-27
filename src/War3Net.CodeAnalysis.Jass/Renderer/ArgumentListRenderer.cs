// ------------------------------------------------------------------------------
// <copyright file="ArgumentListRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassArgumentListSyntax argumentList)
        {
            Render(argumentList.OpenParenToken);

            if (!argumentList.Arguments.Items.IsEmpty)
            {
                Render(argumentList.Arguments.Items[0]);
                for (var i = 1; i < argumentList.Arguments.Items.Length; i++)
                {
                    Render(argumentList.Arguments.Separators[i - 1]);
                    WriteSpace();
                    Render(argumentList.Arguments.Items[i]);
                }
            }

            Render(argumentList.CloseParenToken);
        }
    }
}