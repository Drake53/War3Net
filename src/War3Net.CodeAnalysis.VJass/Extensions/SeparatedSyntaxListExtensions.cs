// ------------------------------------------------------------------------------
// <copyright file="SeparatedSyntaxListExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Extensions
{
    public static class SeparatedSyntaxListExtensions
    {
        public static bool IsEquivalentTo<TSyntaxNode>(this SeparatedSyntaxList<TSyntaxNode, VJassSyntaxToken> list, SeparatedSyntaxList<TSyntaxNode, VJassSyntaxToken> other)
            where TSyntaxNode : VJassSyntaxNode
        {
            return list.Items.IsEquivalentTo(other.Items);
        }

        public static void WriteTo<TSyntaxNode>(this SeparatedSyntaxList<TSyntaxNode, VJassSyntaxToken> list, TextWriter writer)
            where TSyntaxNode : VJassSyntaxNode
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

        public static void ProcessTo<TSyntaxNode>(this SeparatedSyntaxList<TSyntaxNode, VJassSyntaxToken> list, TextWriter writer, VJassPreprocessorContext context)
            where TSyntaxNode : VJassSyntaxNode
        {
            if (list.Items.IsEmpty)
            {
                return;
            }

            list.Items[0].ProcessTo(writer, context);
            for (var i = 1; i < list.Items.Length; i++)
            {
                list.Separators[i - 1].ProcessTo(writer, context);
                list.Items[i].ProcessTo(writer, context);
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