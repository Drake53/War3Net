using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Extensions
{
    internal static class VJassSyntaxNodeExtensions
    {
        internal static bool NullableEquivalentTo(this VJassSyntaxNode? objA, VJassSyntaxNode? objB)
        {
            return ReferenceEquals(objA, objB) || objA?.IsEquivalentTo(objB) == true;
        }
    }
}