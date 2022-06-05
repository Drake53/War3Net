// ------------------------------------------------------------------------------
// <copyright file="VJassKeywordDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassKeywordDeclarationSyntax : IScopedDeclarationSyntax
    {
        public VJassKeywordDeclarationSyntax(
            VJassIdentifierNameSyntax keyword)
        {
            Keyword = keyword;
        }

        public VJassIdentifierNameSyntax Keyword { get; }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassKeywordDeclarationSyntax keywordDeclaration
                && Keyword.Equals(keywordDeclaration.Keyword);
        }

        public override string ToString() => $"{VJassKeyword.Keyword} {Keyword}";
    }
}