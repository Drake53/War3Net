// ------------------------------------------------------------------------------
// <copyright file="PlayerEventApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums.Event;

namespace War3Net.Runtime.Common.Api.Enums.Event
{
    public static class PlayerEventApi
    {
        public static readonly PlayerEvent EVENT_PLAYER_STATE_LIMIT = ConvertPlayerEvent((int)PlayerEvent.Type.StateLimit);
        public static readonly PlayerEvent EVENT_PLAYER_ALLIANCE_CHANGED = ConvertPlayerEvent((int)PlayerEvent.Type.AllianceChanged);

        public static readonly PlayerEvent EVENT_PLAYER_DEFEAT = ConvertPlayerEvent((int)PlayerEvent.Type.Defeat);
        public static readonly PlayerEvent EVENT_PLAYER_VICTORY = ConvertPlayerEvent((int)PlayerEvent.Type.Victory);
        public static readonly PlayerEvent EVENT_PLAYER_LEAVE = ConvertPlayerEvent((int)PlayerEvent.Type.Leave);
        public static readonly PlayerEvent EVENT_PLAYER_CHAT = ConvertPlayerEvent((int)PlayerEvent.Type.Chat);
        public static readonly PlayerEvent EVENT_PLAYER_END_CINEMATIC = ConvertPlayerEvent((int)PlayerEvent.Type.EndCinematic);

        public static readonly PlayerEvent EVENT_PLAYER_ARROW_LEFT_DOWN = ConvertPlayerEvent((int)PlayerEvent.Type.ArrowLeftDown);
        public static readonly PlayerEvent EVENT_PLAYER_ARROW_LEFT_UP = ConvertPlayerEvent((int)PlayerEvent.Type.ArrowLeftUp);
        public static readonly PlayerEvent EVENT_PLAYER_ARROW_RIGHT_DOWN = ConvertPlayerEvent((int)PlayerEvent.Type.ArrowRightDown);
        public static readonly PlayerEvent EVENT_PLAYER_ARROW_RIGHT_UP = ConvertPlayerEvent((int)PlayerEvent.Type.ArrowRightUp);
        public static readonly PlayerEvent EVENT_PLAYER_ARROW_DOWN_DOWN = ConvertPlayerEvent((int)PlayerEvent.Type.ArrowDownDown);
        public static readonly PlayerEvent EVENT_PLAYER_ARROW_DOWN_UP = ConvertPlayerEvent((int)PlayerEvent.Type.ArrowDownUp);
        public static readonly PlayerEvent EVENT_PLAYER_ARROW_UP_DOWN = ConvertPlayerEvent((int)PlayerEvent.Type.ArrowUpDown);
        public static readonly PlayerEvent EVENT_PLAYER_ARROW_UP_UP = ConvertPlayerEvent((int)PlayerEvent.Type.ArrowUpUp);
        public static readonly PlayerEvent EVENT_PLAYER_MOUSE_DOWN = ConvertPlayerEvent((int)PlayerEvent.Type.MouseDown);
        public static readonly PlayerEvent EVENT_PLAYER_MOUSE_UP = ConvertPlayerEvent((int)PlayerEvent.Type.MouseUp);
        public static readonly PlayerEvent EVENT_PLAYER_MOUSE_MOVE = ConvertPlayerEvent((int)PlayerEvent.Type.MouseMove);
        public static readonly PlayerEvent EVENT_PLAYER_SYNC_DATA = ConvertPlayerEvent((int)PlayerEvent.Type.SyncData);
        public static readonly PlayerEvent EVENT_PLAYER_KEY = ConvertPlayerEvent((int)PlayerEvent.Type.Key);
        public static readonly PlayerEvent EVENT_PLAYER_KEY_DOWN = ConvertPlayerEvent((int)PlayerEvent.Type.KeyDown);
        public static readonly PlayerEvent EVENT_PLAYER_KEY_UP = ConvertPlayerEvent((int)PlayerEvent.Type.KeyUp);

        public static PlayerEvent ConvertPlayerEvent(int i)
        {
            return PlayerEvent.GetPlayerEvent(i);
        }
    }
}