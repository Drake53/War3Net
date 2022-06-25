// ------------------------------------------------------------------------------
// <copyright file="ImmutableArrayExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.IO;
using System.Text;

using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Extensions
{
    public static class ImmutableArrayExtensions
    {
        public static bool IsEquivalentTo<TSyntaxNode>(this ImmutableArray<TSyntaxNode> array, ImmutableArray<TSyntaxNode> other)
            where TSyntaxNode : VJassSyntaxNode
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
            where TSyntaxNode : VJassSyntaxNode
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i].WriteTo(writer);
            }
        }

        public static void ProcessTo<TSyntaxNode>(this ImmutableArray<TSyntaxNode> array, TextWriter writer, VJassPreprocessorContext context)
            where TSyntaxNode : VJassSyntaxNode
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i].ProcessTo(writer, context);
            }
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

        internal static string Join(this ImmutableArray<VJassModifierSyntax> modifiers)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < modifiers.Length; i++)
            {
                sb.Append(modifiers[i]);
                sb.Append(' ');
            }

            return sb.ToString();
        }
    }
}