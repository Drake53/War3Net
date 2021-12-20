// ------------------------------------------------------------------------------
// <copyright file="RectangleMargins.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Common
{
    public sealed class RectangleMargins
    {
        public RectangleMargins(int left, int right, int bottom, int top)
        {
            Left = left;
            Right = right;
            Bottom = bottom;
            Top = top;
        }

        internal RectangleMargins(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public int Left { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public int Top { get; set; }

        public override string ToString() => $"{nameof(Left)} = {Left} {nameof(Right)} = {Right} {nameof(Bottom)} = {Bottom} {nameof(Top)} = {Top}";

        internal void ReadFrom(BinaryReader reader)
        {
            Left = reader.ReadInt32();
            Right = reader.ReadInt32();
            Bottom = reader.ReadInt32();
            Top = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(Left);
            writer.Write(Right);
            writer.Write(Bottom);
            writer.Write(Top);
        }
    }
}