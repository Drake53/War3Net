namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassTypeSyntax type)
        {
            switch (type)
            {
                case JassIdentifierNameSyntax identifierName: Render(identifierName); break;
                case JassPredefinedTypeSyntax predefinedType: Render(predefinedType); break;

                default: throw new NotSupportedException();
            }
        }
    }
}