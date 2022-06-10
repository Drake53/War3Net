// ------------------------------------------------------------------------------
// <copyright file="ImmutableArrayExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.IO;

using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Extensions
{
    public static class ImmutableArrayExtensions
    {
        public static void WriteTo<TSyntaxNode>(this ImmutableArray<TSyntaxNode> array, TextWriter writer)
            where TSyntaxNode : VJassSyntaxNode
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i].WriteTo(writer);
            }
        }
    }
}