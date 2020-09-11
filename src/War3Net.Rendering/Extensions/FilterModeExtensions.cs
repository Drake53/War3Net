// ------------------------------------------------------------------------------
// <copyright file="FilterModeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Veldrid;

using War3Net.Modeling.Enums;

namespace War3Net.Rendering.Extensions
{
    public static class FilterModeExtensions
    {
        public static BlendStateDescription ToBlendStateDescription(this FilterMode filterMode)
        {
            return filterMode switch
            {
                FilterMode.None => BlendStateDescription.SingleOverrideBlend,
                FilterMode.Blend => BlendStateDescription.SingleAlphaBlend,
                FilterMode.Additive => BlendStateDescription.SingleAdditiveBlend,
            };
        }
    }
}