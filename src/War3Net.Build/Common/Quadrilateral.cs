// ------------------------------------------------------------------------------
// <copyright file="Quadrilateral.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;
using System.IO;
using System.Text;

namespace War3Net.Build.Common
{
    public sealed class Quadrilateral
    {
        private PointF _bottomleft;
        private PointF _topright;
        private PointF _topleft;
        private PointF _bottomright;

        public Quadrilateral(float left, float right, float top, float bottom)
        {
            _bottomleft = new PointF(left, bottom);
            _topright = new PointF(right, top);
            _topleft = new PointF(left, top);
            _bottomright = new PointF(right, bottom);
        }

        public Quadrilateral(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            _bottomleft = new PointF(x1, y1);
            _topright = new PointF(x2, y2);
            _topleft = new PointF(x3, y3);
            _bottomright = new PointF(x4, y4);
        }

        public Quadrilateral(PointF bottomleft, PointF topright, PointF topleft, PointF bottomright)
        {
            _bottomleft = bottomleft;
            _topright = topright;
            _topleft = topleft;
            _bottomright = bottomright;
        }

        public PointF BottomLeft => _bottomleft;

        public PointF TopRight => _topright;

        public PointF TopLeft => _topleft;

        public PointF BottomRight => _bottomright;

        public static Quadrilateral Parse(Stream stream, bool leaveOpen = false)
        {
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                return new Quadrilateral(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle());
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
            writer.Write(_bottomleft.X);
            writer.Write(_bottomleft.Y);
            writer.Write(_topright.X);
            writer.Write(_topright.Y);
            writer.Write(_topleft.X);
            writer.Write(_topleft.Y);
            writer.Write(_bottomright.X);
            writer.Write(_bottomright.Y);
        }
    }
}