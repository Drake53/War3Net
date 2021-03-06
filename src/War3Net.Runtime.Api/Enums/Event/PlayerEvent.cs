// ------------------------------------------------------------------------------
// <copyright file="PlayerEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace War3Net.Runtime.Api.Enums
{
    public class PlayerEvent : EventId
    {
        /// @CSharpLua.Template = "EVENT_PLAYER_STATE_LIMIT"
        public static readonly PlayerEvent StateLimit;

        /// @CSharpLua.Template = "EVENT_PLAYER_ALLIANCE_CHANGED"
        public static readonly PlayerEvent AllianceChanged;

        /// @CSharpLua.Template = "EVENT_PLAYER_DEFEAT"
        public static readonly PlayerEvent Defeat;

        /// @CSharpLua.Template = "EVENT_PLAYER_VICTORY"
        public static readonly PlayerEvent Victory;

        /// @CSharpLua.Template = "EVENT_PLAYER_LEAVE"
        public static readonly PlayerEvent Leave;

        /// @CSharpLua.Template = "EVENT_PLAYER_CHAT"
        public static readonly PlayerEvent Chat;

        /// @CSharpLua.Template = "EVENT_PLAYER_END_CINEMATIC"
        public static readonly PlayerEvent EndCinematic;

        /// @CSharpLua.Template = "EVENT_PLAYER_ARROW_LEFT_DOWN"
        public static readonly PlayerEvent ArrowLeftDown;

        /// @CSharpLua.Template = "EVENT_PLAYER_ARROW_LEFT_UP"
        public static readonly PlayerEvent ArrowLeftUp;

        /// @CSharpLua.Template = "EVENT_PLAYER_ARROW_RIGHT_DOWN"
        public static readonly PlayerEvent ArrowRightDown;

        /// @CSharpLua.Template = "EVENT_PLAYER_ARROW_RIGHT_UP"
        public static readonly PlayerEvent ArrowRightUp;

        /// @CSharpLua.Template = "EVENT_PLAYER_ARROW_DOWN_DOWN"
        public static readonly PlayerEvent ArrowDownDown;

        /// @CSharpLua.Template = "EVENT_PLAYER_ARROW_DOWN_UP"
        public static readonly PlayerEvent ArrowDownUp;

        /// @CSharpLua.Template = "EVENT_PLAYER_ARROW_UP_DOWN"
        public static readonly PlayerEvent ArrowUpDown;

        /// @CSharpLua.Template = "EVENT_PLAYER_ARROW_UP_UP"
        public static readonly PlayerEvent ArrowUpUp;

        /// @CSharpLua.Template = "EVENT_PLAYER_MOUSE_DOWN"
        public static readonly PlayerEvent MouseDown;

        /// @CSharpLua.Template = "EVENT_PLAYER_MOUSE_UP"
        public static readonly PlayerEvent MouseUp;

        /// @CSharpLua.Template = "EVENT_PLAYER_MOUSE_MOVE"
        public static readonly PlayerEvent MouseMove;

        /// @CSharpLua.Template = "EVENT_PLAYER_SYNC_DATA"
        public static readonly PlayerEvent SyncData;

        /// @CSharpLua.Template = "EVENT_PLAYER_KEY"
        public static readonly PlayerEvent Key;

        /// @CSharpLua.Template = "EVENT_PLAYER_KEY_DOWN"
        public static readonly PlayerEvent KeyDown;

        /// @CSharpLua.Template = "EVENT_PLAYER_KEY_UP"
        public static readonly PlayerEvent KeyUp;

        /// @CSharpLua.Template = "ConvertPlayerEvent({0})"
        public extern PlayerEvent(int id);

        /// @CSharpLua.Template = "ConvertPlayerEvent({0})"
        public static extern PlayerEvent GetById(int id);
    }
}