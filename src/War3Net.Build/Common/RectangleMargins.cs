// ------------------------------------------------------------------------------
// <copyright file="RectangleMargins.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Build
{
    public sealed class RectangleMargins
    {
        private int _left;
        private int _right;
        private int _top;
        private int _bottom;

        public RectangleMargins(int left, int right, int top, int bottom)
        {
            _left = left;
            _right = right;
            _top = top;
            _bottom = bottom;
        }

        public int Left
        {
            get => _left;
            set => _left = value;
        }

        public int Right
        {
            get => _right;
            set => _right = value;
        }

        public int Top
        {
            get => _top;
            set => _top = value;
        }

        public int Bottom
        {
            get => _bottom;
            set => _bottom = value;
        }

        public static RectangleMargins Parse(Stream stream, bool leaveOpen = false)
        {
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                return new RectangleMargins(
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32());
            }
        }

        public void SerializeTo(Stream stream, bool leaveOpen = true)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                WriteTo(writer);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_left);
            writer.Write(_right);
            writer.Write(_top);
            writer.Write(_bottom);
        }
    }
}