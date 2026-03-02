namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassTypeSyntaxExtensions
    {
        public static JassSyntaxToken GetToken(this JassTypeSyntax type)
        {
            if (type is JassIdentifierNameSyntax identifierName)
            {
                return identifierName.Token;
            }
            else if (type is JassPredefinedTypeSyntax predefinedType)
            {
                return predefinedType.Token;
            }
            else
            {
                throw new ArgumentException("Unknown type.", nameof(type));
            }
        }
    }
}