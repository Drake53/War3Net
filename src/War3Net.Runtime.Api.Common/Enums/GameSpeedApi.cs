// ------------------------------------------------------------------------------
// <copyright file="GameSpeedApi.cs" company="Drake53">
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
    public static class GameSpeedApi
    {
        public static readonly GameSpeed MAP_SPEED_SLOWEST = ConvertGameSpeed((int)GameSpeed.Type.Slowest);
        public static readonly GameSpeed MAP_SPEED_SLOW = ConvertGameSpeed((int)GameSpeed.Type.Slow);
        public static readonly GameSpeed MAP_SPEED_NORMAL = ConvertGameSpeed((int)GameSpeed.Type.Normal);
        public static readonly GameSpeed MAP_SPEED_FAST = ConvertGameSpeed((int)GameSpeed.Type.Fast);
        public static readonly GameSpeed MAP_SPEED_FASTEST = ConvertGameSpeed((int)GameSpeed.Type.Fastest);

        public static GameSpeed ConvertGameSpeed(int i)
        {
            return GameSpeed.GetGameSpeed(i);
        }
    }
}