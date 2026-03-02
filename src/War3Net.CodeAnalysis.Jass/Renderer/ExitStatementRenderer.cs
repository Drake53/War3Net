using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassExitStatementSyntax exitStatement)
        {
            Render(exitStatement.ExitWhenToken);
            WriteSpace();
            Render(exitStatement.Condition);
        }
    }
}