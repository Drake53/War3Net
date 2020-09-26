// ------------------------------------------------------------------------------
// <copyright file="UnitStringFieldApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums.Object;

namespace War3Net.Runtime.Api.Common.Enums.Object
{
    public static class UnitStringFieldApi
    {
        public static readonly UnitStringField UNIT_SF_NAME = ConvertUnitStringField((int)UnitStringField.Type.NAME);
        public static readonly UnitStringField UNIT_SF_PROPER_NAMES = ConvertUnitStringField((int)UnitStringField.Type.PROPER_NAMES);
        public static readonly UnitStringField UNIT_SF_GROUND_TEXTURE = ConvertUnitStringField((int)UnitStringField.Type.GROUND_TEXTURE);
        public static readonly UnitStringField UNIT_SF_SHADOW_IMAGE_UNIT = ConvertUnitStringField((int)UnitStringField.Type.SHADOW_IMAGE_UNIT);

        public static UnitStringField ConvertUnitStringField(int i)
        {
            return UnitStringField.GetUnitStringField(i);
        }
    }
}