namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassGlobalsDeclarationSyntax globalsDeclaration)
        {
            Render(globalsDeclaration.GlobalsToken);
            Indent();

            foreach (var globalDeclaration in globalsDeclaration.GlobalDeclarations)
            {
                Render(globalDeclaration);
            }

            Outdent();
            Render(globalsDeclaration.EndGlobalsToken);
        }
    }
}