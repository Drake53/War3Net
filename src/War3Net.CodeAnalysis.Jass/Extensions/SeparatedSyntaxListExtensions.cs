// ------------------------------------------------------------------------------
// <copyright file="SeparatedSyntaxListExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using OneOf;

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

        public static IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetChildNodesAndTokens<TSyntaxNode>(this SeparatedSyntaxList<TSyntaxNode, JassSyntaxToken> list)
            where TSyntaxNode : JassSyntaxNode
        {
            if (list.Items.IsEmpty)
            {
                yield break;
            }

            yield return list.Items[0];
            for (var i = 1; i < list.Items.Length; i++)
            {
                yield return list.Separators[i - 1];
                yield return list.Items[i];
            }
        }

        public static IEnumerable<JassSyntaxNode> GetDescendantNodes<TSyntaxNode>(this SeparatedSyntaxList<TSyntaxNode, JassSyntaxToken> list)
            where TSyntaxNode : JassSyntaxNode
        {
            return list.Items.SelectMany(syntaxNode => syntaxNode.GetDescendantNodes());
        }

        public static IEnumerable<JassSyntaxToken> GetDescendantTokens<TSyntaxNode>(this SeparatedSyntaxList<TSyntaxNode, JassSyntaxToken> list)
            where TSyntaxNode : JassSyntaxNode
        {
            if (list.Items.IsEmpty)
            {
                yield break;
            }

            foreach (var descendant in list.Items[0].GetDescendantTokens())
            {
                yield return descendant;
            }

            for (var i = 1; i < list.Items.Length; i++)
            {
                yield return list.Separators[i - 1];
                foreach (var descendant in list.Items[i].GetDescendantTokens())
                {
                    yield return descendant;
                }
            }
        }

        public static IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens<TSyntaxNode>(this SeparatedSyntaxList<TSyntaxNode, JassSyntaxToken> list)
            where TSyntaxNode : JassSyntaxNode
        {
            if (list.Items.IsEmpty)
            {
                yield break;
            }

            foreach (var descendant in list.Items[0].GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            for (var i = 1; i < list.Items.Length; i++)
            {
                yield return list.Separators[i - 1];
                foreach (var descendant in list.Items[i].GetDescendantNodesAndTokens())
                {
                    yield return descendant;
                }
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