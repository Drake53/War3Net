namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassPredefinedTypeSyntax PredefinedType(JassSyntaxToken keyword)
        {
            ThrowHelper.ThrowIfInvalidPredefinedTypeToken(keyword);

            return new JassPredefinedTypeSyntax(keyword);
        }
    }
}