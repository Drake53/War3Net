using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassSyntaxTrivia trivia)
        {
            if (trivia.SyntaxKind == JassSyntaxKind.SingleLineCommentTrivia)
            {
                WriteSpace();
                Write(trivia.Text.TrimEnd());
            }
            else if (trivia.SyntaxKind == JassSyntaxKind.NewLineTrivia)
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