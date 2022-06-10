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
    }
}