// ------------------------------------------------------------------------------
// <copyright file="GameEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace War3Net.Runtime.Api.Enums
{
    public class GameEvent : EventId
    {
        /// @CSharpLua.Template = "EVENT_GAME_VICTORY"
        public static readonly GameEvent Victory;

        /// @CSharpLua.Template = "EVENT_GAME_END_LEVEL"
        public static readonly GameEvent EndLevel;

        /// @CSharpLua.Template = "EVENT_GAME_VARIABLE_LIMIT"
        public static readonly GameEvent VariableLimit;

        /// @CSharpLua.Template = "EVENT_GAME_STATE_LIMIT"
        public static readonly GameEvent StateLimit;

        /// @CSharpLua.Template = "EVENT_GAME_TIMER_EXPIRED"
        public static readonly GameEvent TimerExpired;

        /// @CSharpLua.Template = "EVENT_GAME_ENTER_REGION"
        public static readonly GameEvent EnterRegion;

        /// @CSharpLua.Template = "EVENT_GAME_LEAVE_REGION"
        public static readonly GameEvent LeaveRegion;

        /// @CSharpLua.Template = "EVENT_GAME_TRACKABLE_HIT"
        public static readonly GameEvent TrackableHit;

        /// @CSharpLua.Template = "EVENT_GAME_TRACKABLE_TRACK"
        public static readonly GameEvent TrackableTrack;

        /// @CSharpLua.Template = "EVENT_GAME_SHOW_SKILL"
        public static readonly GameEvent ShowSkill;

        /// @CSharpLua.Template = "EVENT_GAME_BUILD_SUBMENU"
        public static readonly GameEvent BuildSubMenu;

        /// @CSharpLua.Template = "EVENT_GAME_LOADED"
        public static readonly GameEvent Loaded;

        /// @CSharpLua.Template = "EVENT_GAME_TOURNAMENT_FINISH_SOON"
        public static readonly GameEvent TournamentFinishSoon;

        /// @CSharpLua.Template = "EVENT_GAME_TOURNAMENT_FINISH_NOW"
        public static readonly GameEvent TournamentFinishNow;

        /// @CSharpLua.Template = "EVENT_GAME_SAVE"
        public static readonly GameEvent Save;

        /// @CSharpLua.Template = "EVENT_GAME_CUSTOM_UI_FRAME"
        public static readonly GameEvent CustomUIFrame;

        /// @CSharpLua.Template = "ConvertGameEvent({0})"
        public extern GameEvent(int id);

        /// @CSharpLua.Template = "ConvertGameEvent({0})"
        public static extern GameEvent GetById(int id);
    }
}