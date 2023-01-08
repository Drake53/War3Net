// ------------------------------------------------------------------------------
// <copyright file="RectangleMargins.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Common
{
    public sealed partial class RectangleMargins
    {
        public RectangleMargins(int left, int right, int bottom, int top)
        {
            Left = left;
            Right = right;
            Bottom = bottom;
            Top = top;
        }

        public int Left { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public int Top { get; set; }

        public override string ToString() => $"{nameof(Left)} = {Left} {nameof(Right)} = {Right} {nameof(Bottom)} = {Bottom} {nameof(Top)} = {Top}";
    }
}