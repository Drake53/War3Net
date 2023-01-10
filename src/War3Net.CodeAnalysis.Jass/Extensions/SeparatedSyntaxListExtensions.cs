// ------------------------------------------------------------------------------
// <copyright file="SeparatedSyntaxListExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class SeparatedSyntaxListExtensions
    {
        public static bool IsEquivalentTo<TSyntaxNode>(this SeparatedSyntaxList<TSyntaxNode, JassSyntaxToken> list, SeparatedSyntaxList<TSyntaxNode, JassSyntaxToken> other)
            where TSyntaxNode : JassSyntaxNode
        {
            return list.Items.IsEquivalentTo(other.Items);
        }

        public static void WriteTo<TSyntaxNode>(this SeparatedSyntaxList<TSyntaxNode, JassSyntaxToken> list, TextWriter writer)
            where TSyntaxNode : JassSyntaxNode
        {
            if (list.Items.IsEmpty)
            {
                return;
            }

            list.Items[0].WriteTo(writer);
            for (var i = 1; i < list.Items.Length; i++)
            {
                list.Separators[i - 1].WriteTo(writer);
                list.Items[i].WriteTo(writer);
            }
        }

        internal static SeparatedSyntaxList<TItem, TSeparator> ReplaceFirstItem<TItem, TSeparator>(this SeparatedSyntaxList<TItem, TSeparator> list, TItem newItem)
        {
            var items = list.Items.ReplaceFirstItem(newItem);
            return SeparatedSyntaxList<TItem, TSeparator>.Create(items, list.Separators);
        }

        internal static SeparatedSyntaxList<TItem, TSeparator> ReplaceLastItem<TItem, TSeparator>(this SeparatedSyntaxList<TItem, TSeparator> list, TItem newItem)
        {
            var items = list.Items.ReplaceLastItem(newItem);
            return SeparatedSyntaxList<TItem, TSeparator>.Create(items, list.Separators);
        }
    }
}