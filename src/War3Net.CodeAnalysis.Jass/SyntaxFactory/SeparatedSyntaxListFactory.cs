// ------------------------------------------------------------------------------
// <copyright file="SeparatedSyntaxListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static SeparatedSyntaxList<TItem, JassSyntaxToken> SeparatedSyntaxList<TItem>(JassSyntaxKind syntaxKind, params TItem[] items)
            where TItem : JassSyntaxNode
        {
            if (items.Length == 0)
            {
                return SeparatedSyntaxList<TItem, JassSyntaxToken>.Empty;
            }

            var builder = SeparatedSyntaxList<TItem, JassSyntaxToken>.CreateBuilder(items[0]);
            for (var i = 1; i < items.Length; i++)
            {
                builder.Add(Token(syntaxKind), items[i]);
            }

            return builder.ToSeparatedSyntaxList();
        }
    }
}