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

            if (!argumentList.ArgumentList.Items.IsEmpty)
            {
                Render(argumentList.ArgumentList.Items[0]);
                for (var i = 1; i < argumentList.ArgumentList.Items.Length; i++)
                {
                    Render(argumentList.ArgumentList.Separators[i - 1]);
                    WriteSpace();
                    Render(argumentList.ArgumentList.Items[i]);
                }
            }

            Render(argumentList.CloseParenToken);
        }
    }
}