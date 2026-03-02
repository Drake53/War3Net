namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassSyntaxNodeExtensions
    {
        public static bool NullableEquivalentTo(this JassSyntaxNode? objA, JassSyntaxNode? objB)
        {
            return ReferenceEquals(objA, objB) || objA?.IsEquivalentTo(objB) == true;
        }
    }
}