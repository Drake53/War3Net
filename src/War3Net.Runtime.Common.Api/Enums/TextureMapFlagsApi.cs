// ------------------------------------------------------------------------------
// <copyright file="TextureMapFlagsApi.cs" company="Drake53">
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
    public static class TextureMapFlagsApi
    {
        public static readonly TextureMapFlags TEXMAP_FLAG_NONE = ConvertTexMapFlags((int)TextureMapFlags.Type.None);
        public static readonly TextureMapFlags TEXMAP_FLAG_WRAP_U = ConvertTexMapFlags((int)TextureMapFlags.Type.WrapU);
        public static readonly TextureMapFlags TEXMAP_FLAG_WRAP_V = ConvertTexMapFlags((int)TextureMapFlags.Type.WrapV);
        public static readonly TextureMapFlags TEXMAP_FLAG_WRAP_UV = ConvertTexMapFlags((int)TextureMapFlags.Type.WrapUV);

        public static TextureMapFlags ConvertTexMapFlags(int i)
        {
            return TextureMapFlags.GetTextureMapFlags(i);
        }
    }
}