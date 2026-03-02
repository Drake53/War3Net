namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassArrayDeclaratorSyntax arrayDeclarator)
        {
            Render(arrayDeclarator.Type);
            WriteSpace();
            Render(arrayDeclarator.ArrayToken);
            WriteSpace();
            Render(arrayDeclarator.IdentifierName);
        }
    }
}