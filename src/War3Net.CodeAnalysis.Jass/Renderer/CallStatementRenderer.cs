using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassCallStatementSyntax callStatement)
        {
            Render(callStatement.CallToken);
            WriteSpace();
            Render(callStatement.IdentifierName);
            Render(callStatement.ArgumentList);
        }
    }
}