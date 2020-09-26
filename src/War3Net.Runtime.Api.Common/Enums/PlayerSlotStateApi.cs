// ------------------------------------------------------------------------------
// <copyright file="PlayerSlotStateApi.cs" company="Drake53">
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
    public static class PlayerSlotStateApi
    {
        public static readonly PlayerSlotState PLAYER_SLOT_STATE_EMPTY = ConvertPlayerSlotState((int)PlayerSlotState.Type.Empty);
        public static readonly PlayerSlotState PLAYER_SLOT_STATE_PLAYING = ConvertPlayerSlotState((int)PlayerSlotState.Type.Playing);
        public static readonly PlayerSlotState PLAYER_SLOT_STATE_LEFT = ConvertPlayerSlotState((int)PlayerSlotState.Type.Left);

        public static PlayerSlotState ConvertPlayerSlotState(int i)
        {
            return PlayerSlotState.GetPlayerSlotState(i);
        }
    }
}