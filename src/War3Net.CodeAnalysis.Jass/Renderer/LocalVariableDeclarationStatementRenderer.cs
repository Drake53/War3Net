using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement)
        {
            Render(localVariableDeclarationStatement.LocalToken);
            WriteSpace();
            Render(localVariableDeclarationStatement.Declarator);
        }
    }
}