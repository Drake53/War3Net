using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassSyntaxTriviaList triviaList)
        {
            foreach (var trivia in triviaList.Trivia)
            {
                Render(trivia);
            }
        }
    }
}