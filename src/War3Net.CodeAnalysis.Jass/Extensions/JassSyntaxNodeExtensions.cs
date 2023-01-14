// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxNodeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

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