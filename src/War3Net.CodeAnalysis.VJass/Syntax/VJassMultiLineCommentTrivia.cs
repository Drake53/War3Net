// ------------------------------------------------------------------------------
// <copyright file="VJassMultiLineCommentTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMultiLineCommentTrivia : ISyntaxTrivia, IMultiLineCommentContent
    {
        public VJassMultiLineCommentTrivia(ImmutableArray<IMultiLineCommentContent> content)
        {
            Content = content;
        }

        public ImmutableArray<IMultiLineCommentContent> Content { get; }

        public bool Equals(ISyntaxTrivia? other)
        {
            return other is VJassMultiLineCommentTrivia commentTrivia
                && Content.SequenceEqual(commentTrivia.Content);
        }

        public bool Equals(IMultiLineCommentContent? other)
        {
            return other is VJassMultiLineCommentTrivia commentTrivia
                && Content.SequenceEqual(commentTrivia.Content);
        }

        public override string ToString() => $"{VJassSymbol.Slash}{VJassSymbol.Asterisk}{string.Concat(Content)}{VJassSymbol.Asterisk}{VJassSymbol.Slash}";
    }
}