// ------------------------------------------------------------------------------
// <copyright file="Quadrilateral.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Numerics;

namespace War3Net.Build.Common
{
    public sealed partial class Quadrilateral
    {
        public Quadrilateral(float left, float right, float top, float bottom)
        {
            BottomLeft = new Vector2(left, bottom);
            TopRight = new Vector2(right, top);
            TopLeft = new Vector2(left, top);
            BottomRight = new Vector2(right, bottom);
        }

        public Quadrilateral(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            BottomLeft = new Vector2(x1, y1);
            TopRight = new Vector2(x2, y2);
            TopLeft = new Vector2(x3, y3);
            BottomRight = new Vector2(x4, y4);
        }

        public Quadrilateral(Vector2 bottomLeft, Vector2 topRight, Vector2 topLeft, Vector2 bottomRight)
        {
            BottomLeft = bottomLeft;
            TopRight = topRight;
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        internal Quadrilateral(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public Vector2 BottomLeft { get; set; }

        public Vector2 TopRight { get; set; }

        public Vector2 TopLeft { get; set; }

        public Vector2 BottomRight { get; set; }
    }
}