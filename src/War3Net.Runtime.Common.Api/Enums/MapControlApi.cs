// ------------------------------------------------------------------------------
// <copyright file="MapControlApi.cs" company="Drake53">
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
    public static class MapControlApi
    {
        public static readonly MapControl MAP_CONTROL_USER = ConvertMapControl((int)MapControl.Type.User);
        public static readonly MapControl MAP_CONTROL_COMPUTER = ConvertMapControl((int)MapControl.Type.Computer);
        public static readonly MapControl MAP_CONTROL_RESCUABLE = ConvertMapControl((int)MapControl.Type.Rescuable);
        public static readonly MapControl MAP_CONTROL_NEUTRAL = ConvertMapControl((int)MapControl.Type.Neutral);
        public static readonly MapControl MAP_CONTROL_CREEP = ConvertMapControl((int)MapControl.Type.Creep);
        public static readonly MapControl MAP_CONTROL_NONE = ConvertMapControl((int)MapControl.Type.None);

        public static MapControl ConvertMapControl(int i)
        {
            return MapControl.GetMapControl(i);
        }
    }
}