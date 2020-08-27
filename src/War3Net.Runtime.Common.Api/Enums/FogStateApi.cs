// ------------------------------------------------------------------------------
// <copyright file="FogStateApi.cs" company="Drake53">
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
    public static class FogStateApi
    {
        public static readonly FogState FOG_OF_WAR_MASKED = ConvertFogState((int)FogState.Type.Masked);
        public static readonly FogState FOG_OF_WAR_FOGGED = ConvertFogState((int)FogState.Type.Fogged);
        public static readonly FogState FOG_OF_WAR_VISIBLE = ConvertFogState((int)FogState.Type.Visible);

        public static FogState ConvertFogState(int i)
        {
            return FogState.GetFogState(i);
        }
    }
}