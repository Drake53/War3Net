// ------------------------------------------------------------------------------
// <copyright file="ImmutableArrayExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

using OneOf;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class ImmutableArrayExtensions
    {
        public static bool IsEquivalentTo<TSyntaxNode>(this ImmutableArray<TSyntaxNode> array, ImmutableArray<TSyntaxNode> other)
            where TSyntaxNode : JassSyntaxNode
        {
            if (array.Length != other.Length)
            {
                return false;
            }

            for (var i = 0; i < array.Length; i++)
            {
                if (!array[i].IsEquivalentTo(other[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static void WriteTo<TSyntaxNode>(this ImmutableArray<TSyntaxNode> array, TextWriter writer)
            where TSyntaxNode : JassSyntaxNode
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i].WriteTo(writer);
            }
        }

        public static IEnumerable<JassSyntaxNode> GetDescendantNodes<TSyntaxNode>(this ImmutableArray<TSyntaxNode> array)
            where TSyntaxNode : JassSyntaxNode
        {
            return array.SelectMany(item => item.GetDescendantNodes());
        }

        public static IEnumerable<JassSyntaxToken> GetDescendantTokens<TSyntaxNode>(this ImmutableArray<TSyntaxNode> array)
            where TSyntaxNode : JassSyntaxNode
        {
            return array.SelectMany(item => item.GetDescendantTokens());
        }

        public static IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens<TSyntaxNode>(this ImmutableArray<TSyntaxNode> array)
            where TSyntaxNode : JassSyntaxNode
        {
            return array.SelectMany(item => item.GetDescendantNodesAndTokens());
        }

        internal static ImmutableArray<T> ReplaceFirstItem<T>(this ImmutableArray<T> array, T newItem)
        {
            var builder = array.ToBuilder();
            builder[0] = newItem;
            return builder.ToImmutable();
        }

        internal static ImmutableArray<T> ReplaceLastItem<T>(this ImmutableArray<T> array, T newItem)
        {
            var builder = array.ToBuilder();
            builder[^1] = newItem;
            return builder.ToImmutable();
        }
    }
}