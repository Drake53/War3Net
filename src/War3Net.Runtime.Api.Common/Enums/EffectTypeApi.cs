// ------------------------------------------------------------------------------
// <copyright file="EffectTypeApi.cs" company="Drake53">
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
    public static class EffectTypeApi
    {
        public static readonly EffectType EFFECT_TYPE_EFFECT = ConvertEffectType((int)EffectType.Type.Effect);
        public static readonly EffectType EFFECT_TYPE_TARGET = ConvertEffectType((int)EffectType.Type.Target);
        public static readonly EffectType EFFECT_TYPE_CASTER = ConvertEffectType((int)EffectType.Type.Caster);
        public static readonly EffectType EFFECT_TYPE_SPECIAL = ConvertEffectType((int)EffectType.Type.Special);
        public static readonly EffectType EFFECT_TYPE_AREA_EFFECT = ConvertEffectType((int)EffectType.Type.AreaEffect);
        public static readonly EffectType EFFECT_TYPE_MISSILE = ConvertEffectType((int)EffectType.Type.Missile);
        public static readonly EffectType EFFECT_TYPE_LIGHTNING = ConvertEffectType((int)EffectType.Type.Lightning);

        public static EffectType ConvertEffectType(int i)
        {
            return EffectType.GetEffectType(i);
        }
    }
}