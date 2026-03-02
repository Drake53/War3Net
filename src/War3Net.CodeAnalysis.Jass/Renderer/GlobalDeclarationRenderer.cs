namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassGlobalDeclarationSyntax globalDeclaration)
        {
            switch (globalDeclaration)
            {
                case JassGlobalConstantDeclarationSyntax globalConstantDeclaration: Render(globalConstantDeclaration); break;
                case JassGlobalVariableDeclarationSyntax globalVariableDeclaration: Render(globalVariableDeclaration); break;

                default: throw new NotSupportedException();
            }
        }
    }
}