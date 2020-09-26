// ------------------------------------------------------------------------------
// <copyright file="MouseButtonTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class MouseButtonTypeApi
    {
        public static readonly MouseButtonType MOUSE_BUTTON_TYPE_LEFT = ConvertMouseButtonType((int)MouseButtonType.Type.Left);
        public static readonly MouseButtonType MOUSE_BUTTON_TYPE_MIDDLE = ConvertMouseButtonType((int)MouseButtonType.Type.Middle);
        public static readonly MouseButtonType MOUSE_BUTTON_TYPE_RIGHT = ConvertMouseButtonType((int)MouseButtonType.Type.Right);

        public static MouseButtonType ConvertMouseButtonType(int i)
        {
            return MouseButtonType.GetMouseButtonType(i);
        }
    }
}