// ------------------------------------------------------------------------------
// <copyright file="TextAlignTypeApi.cs" company="Drake53">
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
    public static class TextAlignTypeApi
    {
        public static readonly TextAlignType TEXT_JUSTIFY_TOP = ConvertTextAlignType((int)TextAlignType.Type.Top);
        public static readonly TextAlignType TEXT_JUSTIFY_MIDDLE = ConvertTextAlignType((int)TextAlignType.Type.Middle);
        public static readonly TextAlignType TEXT_JUSTIFY_BOTTOM = ConvertTextAlignType((int)TextAlignType.Type.Bottom);
        public static readonly TextAlignType TEXT_JUSTIFY_LEFT = ConvertTextAlignType((int)TextAlignType.Type.Left);
        public static readonly TextAlignType TEXT_JUSTIFY_CENTER = ConvertTextAlignType((int)TextAlignType.Type.Center);
        public static readonly TextAlignType TEXT_JUSTIFY_RIGHT = ConvertTextAlignType((int)TextAlignType.Type.Right);

        public static TextAlignType ConvertTextAlignType(int i)
        {
            return TextAlignType.GetTextAlignType(i);
        }
    }
}