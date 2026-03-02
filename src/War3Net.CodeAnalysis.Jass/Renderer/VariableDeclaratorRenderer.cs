namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassVariableDeclaratorSyntax variableDeclarator)
        {
            Render(variableDeclarator.Type);
            WriteSpace();
            Render(variableDeclarator.IdentifierName);

            if (variableDeclarator.Value is not null)
            {
                WriteSpace();
                Render(variableDeclarator.Value);
            }
        }
    }
}