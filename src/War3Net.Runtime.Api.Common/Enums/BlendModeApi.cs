// ------------------------------------------------------------------------------
// <copyright file="BlendModeApi.cs" company="Drake53">
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
    public static class BlendModeApi
    {
        public static readonly BlendMode BLEND_MODE_NONE = ConvertBlendMode((int)BlendMode.Type.None);
        public static readonly BlendMode BLEND_MODE_DONT_CARE = ConvertBlendMode((int)BlendMode.Type.None);
        public static readonly BlendMode BLEND_MODE_KEYALPHA = ConvertBlendMode((int)BlendMode.Type.Alpha);
        public static readonly BlendMode BLEND_MODE_BLEND = ConvertBlendMode((int)BlendMode.Type.Blend);
        public static readonly BlendMode BLEND_MODE_ADDITIVE = ConvertBlendMode((int)BlendMode.Type.Additive);
        public static readonly BlendMode BLEND_MODE_MODULATE = ConvertBlendMode((int)BlendMode.Type.Modulate);
        public static readonly BlendMode BLEND_MODE_MODULATE_2X = ConvertBlendMode((int)BlendMode.Type.Modulate2x);

        public static BlendMode ConvertBlendMode(int i)
        {
            return BlendMode.GetBlendMode(i);
        }
    }
}