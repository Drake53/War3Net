// ------------------------------------------------------------------------------
// <copyright file="FilterModeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;

using Veldrid;

using War3Net.Modeling.Enums;

namespace War3Net.Rendering.Extensions
{
    public static class FilterModeExtensions
    {
        private static readonly BlendStateDescription None = BlendStateDescription.SingleOverrideBlend;
        private static readonly BlendStateDescription Transparent = new BlendStateDescription(default, true, BlendStateDescription.SingleAlphaBlend.AttachmentStates);
        private static readonly BlendStateDescription Blend = BlendStateDescription.SingleAlphaBlend;
        private static readonly BlendStateDescription Additive = BlendStateDescription.SingleAdditiveBlend;

        public static BlendStateDescription ToBlendStateDescription(this FilterMode filterMode)
        {
            return filterMode switch
            {
                FilterMode.None => None,
                FilterMode.Transparent => Transparent,
                FilterMode.Blend => Blend,
                FilterMode.Additive => Additive,

                FilterMode.AddAlpha => throw new NotImplementedException(),
                FilterMode.Modulate => throw new NotImplementedException(),
                FilterMode.Modulate2x => throw new NotImplementedException(),

                _ => throw new InvalidEnumArgumentException(nameof(filterMode), (int)filterMode, typeof(FilterMode)),
            };
        }
    }
}