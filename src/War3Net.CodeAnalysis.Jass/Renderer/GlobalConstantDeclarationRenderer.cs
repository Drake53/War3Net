namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassGlobalConstantDeclarationSyntax globalConstantDeclaration)
        {
            Render(globalConstantDeclaration.ConstantToken);
            WriteSpace();
            Render(globalConstantDeclaration.Type);
            WriteSpace();
            Render(globalConstantDeclaration.IdentifierName);
            WriteSpace();
            Render(globalConstantDeclaration.Value);
        }
    }
}