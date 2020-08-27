// ------------------------------------------------------------------------------
// <copyright file="UnitStateApi.cs" company="Drake53">
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
    public static class UnitStateApi
    {
        public static readonly UnitState UNIT_STATE_LIFE = ConvertUnitState((int)UnitState.Type.Life);
        public static readonly UnitState UNIT_STATE_MAX_LIFE = ConvertUnitState((int)UnitState.Type.MaxLife);
        public static readonly UnitState UNIT_STATE_MANA = ConvertUnitState((int)UnitState.Type.Mana);
        public static readonly UnitState UNIT_STATE_MAX_MANA = ConvertUnitState((int)UnitState.Type.MaxMana);

        public static UnitState ConvertUnitState(int i)
        {
            return UnitState.GetUnitState(i);
        }
    }
}