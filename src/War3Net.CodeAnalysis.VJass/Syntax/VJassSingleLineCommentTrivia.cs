// ------------------------------------------------------------------------------
// <copyright file="VJassSingleLineCommentTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassSingleLineCommentTrivia : ISyntaxTrivia
    {
        public VJassSingleLineCommentTrivia(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; }

        public bool Equals(ISyntaxTrivia? other)
        {
            return other is VJassSingleLineCommentTrivia commentTrivia
                && string.Equals(Comment, commentTrivia.Comment, StringComparison.Ordinal);
        }

        public override string ToString() => $"{VJassSymbol.Slash}{VJassSymbol.Slash}{Comment}";
    }
}