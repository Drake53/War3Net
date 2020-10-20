// ------------------------------------------------------------------------------
// <copyright file="FramePoint.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.UI
{
    internal sealed class FramePoint
    {
        private float _x;
        private float _y;

        public float X
        {
            get
            {
                if (Relative is null)
                {
                    return _x;
                }

                var rect = Relative.CurrentRect;
                switch (RelativeType)
                {
                    case FramePointType.Type.TopLeft:
                    case FramePointType.Type.Left:
                    case FramePointType.Type.BottomLeft:
                        return _x + rect.X;

                    case FramePointType.Type.Top:
                    case FramePointType.Type.Center:
                    case FramePointType.Type.Bottom:
                        return _x + rect.X + (0.5f * rect.Width);

                    case FramePointType.Type.TopRight:
                    case FramePointType.Type.Right:
                    case FramePointType.Type.BottomRight:
                        return _x + rect.X + rect.Width;

                    default: throw new InvalidEnumArgumentException(nameof(RelativeType), (int)RelativeType, typeof(FramePointType.Type));
                }
            }

            set => _x = value;
        }

        public float Y
        {
            get
            {
                if (Relative is null)
                {
                    return _y;
                }

                var rect = Relative.CurrentRect;
                switch (RelativeType)
                {
                    case FramePointType.Type.TopLeft:
                    case FramePointType.Type.Top:
                    case FramePointType.Type.TopRight:
                        return _y + rect.Y + rect.Height;

                    case FramePointType.Type.Left:
                    case FramePointType.Type.Center:
                    case FramePointType.Type.Right:
                        return _y + rect.Y + (0.5f * rect.Height);

                    case FramePointType.Type.BottomLeft:
                    case FramePointType.Type.Bottom:
                    case FramePointType.Type.BottomRight:
                        return _y + rect.Y;

                    default: throw new InvalidEnumArgumentException(nameof(RelativeType), (int)RelativeType, typeof(FramePointType.Type));
                }
            }

            set => _y = value;
        }

        public FrameHandle? Relative { get; set; }

        public FramePointType.Type RelativeType { get; set; }

        internal bool Usable { get; set; }
    }
}