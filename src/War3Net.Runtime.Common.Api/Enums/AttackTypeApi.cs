// ------------------------------------------------------------------------------
// <copyright file="AttackTypeApi.cs" company="Drake53">
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
    public static class AttackTypeApi
    {
        public static readonly AttackType ATTACK_TYPE_NORMAL = ConvertAttackType((int)AttackType.Type.Normal);
        public static readonly AttackType ATTACK_TYPE_MELEE = ConvertAttackType((int)AttackType.Type.Melee);
        public static readonly AttackType ATTACK_TYPE_PIERCE = ConvertAttackType((int)AttackType.Type.Pierce);
        public static readonly AttackType ATTACK_TYPE_SIEGE = ConvertAttackType((int)AttackType.Type.Siege);
        public static readonly AttackType ATTACK_TYPE_MAGIC = ConvertAttackType((int)AttackType.Type.Magic);
        public static readonly AttackType ATTACK_TYPE_CHAOS = ConvertAttackType((int)AttackType.Type.Chaos);
        public static readonly AttackType ATTACK_TYPE_HERO = ConvertAttackType((int)AttackType.Type.Hero);

        public static AttackType ConvertAttackType(int i)
        {
            return AttackType.GetAttackType(i);
        }
    }
}