// ------------------------------------------------------------------------------
// <copyright file="FramePointTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
{
    public static class FramePointTypeApi
    {
        public static readonly FramePointType FRAMEPOINT_TOPLEFT = ConvertFramePointType((int)FramePointType.Type.TopLeft);
        public static readonly FramePointType FRAMEPOINT_TOP = ConvertFramePointType((int)FramePointType.Type.Top);
        public static readonly FramePointType FRAMEPOINT_TOPRIGHT = ConvertFramePointType((int)FramePointType.Type.TopRight);
        public static readonly FramePointType FRAMEPOINT_LEFT = ConvertFramePointType((int)FramePointType.Type.Left);
        public static readonly FramePointType FRAMEPOINT_CENTER = ConvertFramePointType((int)FramePointType.Type.Center);
        public static readonly FramePointType FRAMEPOINT_RIGHT = ConvertFramePointType((int)FramePointType.Type.Right);
        public static readonly FramePointType FRAMEPOINT_BOTTOMLEFT = ConvertFramePointType((int)FramePointType.Type.BottomLeft);
        public static readonly FramePointType FRAMEPOINT_BOTTOM = ConvertFramePointType((int)FramePointType.Type.Bottom);
        public static readonly FramePointType FRAMEPOINT_BOTTOMRIGHT = ConvertFramePointType((int)FramePointType.Type.BottomRight);

        public static FramePointType ConvertFramePointType(int i)
        {
            return FramePointType.GetFramePointType(i);
        }
    }
}