using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassSyntaxToken syntaxToken)
        {
            Render(syntaxToken.LeadingTrivia);
            Write(syntaxToken.Text);
            Render(syntaxToken.TrailingTrivia);
        }
    }
}