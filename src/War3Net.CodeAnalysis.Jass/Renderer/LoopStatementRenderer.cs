using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassLoopStatementSyntax loopStatement)
        {
            Render(loopStatement.LoopToken);
            Indent();
            Render(loopStatement.Statements);
            Outdent();
            Render(loopStatement.EndLoopToken);
        }
    }
}