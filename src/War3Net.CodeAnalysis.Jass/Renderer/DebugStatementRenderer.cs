namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassDebugStatementSyntax debugStatement)
        {
            Render(debugStatement.DebugToken);
            WriteSpace();
            Render(debugStatement.Statement);
        }
    }
}