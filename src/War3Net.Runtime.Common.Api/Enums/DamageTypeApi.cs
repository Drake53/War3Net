// ------------------------------------------------------------------------------
// <copyright file="DamageTypeApi.cs" company="Drake53">
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
    public static class DamageTypeApi
    {
        public static readonly DamageType DAMAGE_TYPE_UNKNOWN = ConvertDamageType((int)DamageType.Type.Unknown);
        public static readonly DamageType DAMAGE_TYPE_NORMAL = ConvertDamageType((int)DamageType.Type.Normal);
        public static readonly DamageType DAMAGE_TYPE_ENHANCED = ConvertDamageType((int)DamageType.Type.Enhanced);
        public static readonly DamageType DAMAGE_TYPE_FIRE = ConvertDamageType((int)DamageType.Type.Fire);
        public static readonly DamageType DAMAGE_TYPE_COLD = ConvertDamageType((int)DamageType.Type.Cold);
        public static readonly DamageType DAMAGE_TYPE_LIGHTNING = ConvertDamageType((int)DamageType.Type.Lightning);
        public static readonly DamageType DAMAGE_TYPE_POISON = ConvertDamageType((int)DamageType.Type.Poison);
        public static readonly DamageType DAMAGE_TYPE_DISEASE = ConvertDamageType((int)DamageType.Type.Disease);
        public static readonly DamageType DAMAGE_TYPE_DIVINE = ConvertDamageType((int)DamageType.Type.Divine);
        public static readonly DamageType DAMAGE_TYPE_MAGIC = ConvertDamageType((int)DamageType.Type.Magic);
        public static readonly DamageType DAMAGE_TYPE_SONIC = ConvertDamageType((int)DamageType.Type.Sonic);
        public static readonly DamageType DAMAGE_TYPE_ACID = ConvertDamageType((int)DamageType.Type.Acid);
        public static readonly DamageType DAMAGE_TYPE_FORCE = ConvertDamageType((int)DamageType.Type.Force);
        public static readonly DamageType DAMAGE_TYPE_DEATH = ConvertDamageType((int)DamageType.Type.Death);
        public static readonly DamageType DAMAGE_TYPE_MIND = ConvertDamageType((int)DamageType.Type.Mind);
        public static readonly DamageType DAMAGE_TYPE_PLANT = ConvertDamageType((int)DamageType.Type.Plant);
        public static readonly DamageType DAMAGE_TYPE_DEFENSIVE = ConvertDamageType((int)DamageType.Type.Defensive);
        public static readonly DamageType DAMAGE_TYPE_DEMOLITION = ConvertDamageType((int)DamageType.Type.Demolition);
        public static readonly DamageType DAMAGE_TYPE_SLOW_POISON = ConvertDamageType((int)DamageType.Type.SlowPoison);
        public static readonly DamageType DAMAGE_TYPE_SPIRIT_LINK = ConvertDamageType((int)DamageType.Type.SpiritLink);
        public static readonly DamageType DAMAGE_TYPE_SHADOW_STRIKE = ConvertDamageType((int)DamageType.Type.ShadowStrike);
        public static readonly DamageType DAMAGE_TYPE_UNIVERSAL = ConvertDamageType((int)DamageType.Type.Universal);

        public static DamageType ConvertDamageType(int i)
        {
            return DamageType.GetDamageType(i);
        }
    }
}