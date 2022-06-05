// ------------------------------------------------------------------------------
// <copyright file="VJassMultiLineCommentText.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMultiLineCommentText : IMultiLineCommentContent
    {
        public VJassMultiLineCommentText(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public bool Equals(IMultiLineCommentContent? other)
        {
            return other is VJassMultiLineCommentText commentText
                && string.Equals(Text, commentText.Text, StringComparison.Ordinal);
        }

        public override string ToString() => Text;
    }
}