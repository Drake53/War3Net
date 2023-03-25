// ------------------------------------------------------------------------------
// <copyright file="SyntaxTriviaListRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassSyntaxTriviaList syntaxTriviaList)
        {
            foreach (var trivia in syntaxTriviaList.Trivia)
            {
                if (trivia.SyntaxKind == JassSyntaxKind.SingleLineCommentTrivia)
                {
                    WriteSpace();
                    Write(trivia.Text.TrimEnd());
                }
                else if (trivia.SyntaxKind == JassSyntaxKind.NewlineTrivia)
                {
                    var lines = 0;
                    var isCarriageReturn = false;
                    for (var i = 0; i < trivia.Text.Length; i++)
                    {
                        if (trivia.Text[i] == '\r')
                        {
                            if (isCarriageReturn)
                            {
                                lines++;
                            }
                            else
                            {
                                isCarriageReturn = true;
                            }
                        }
                        else
                        {
                            lines++;
                            isCarriageReturn = false;
                        }
                    }

                    if (isCarriageReturn)
                    {
                        lines++;
                    }

                    for (var i = 0; i < lines; i++)
                    {
                        WriteLine();
                    }
                }
            }
        }
    }
}