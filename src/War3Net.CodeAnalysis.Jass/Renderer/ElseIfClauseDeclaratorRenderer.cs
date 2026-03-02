namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator)
        {
            Render(elseIfClauseDeclarator.ElseIfToken);
            WriteSpace();
            Render(elseIfClauseDeclarator.Condition);
            WriteSpace();
            Render(elseIfClauseDeclarator.ThenToken);
        }
    }
}