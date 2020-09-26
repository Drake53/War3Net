// ------------------------------------------------------------------------------
// <copyright file="GameEventApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums.Event;

namespace War3Net.Runtime.Api.Common.Enums.Event
{
    public static class GameEventApi
    {
        public static readonly GameEvent EVENT_GAME_VICTORY = ConvertGameEvent((int)GameEvent.Type.Victory);
        public static readonly GameEvent EVENT_GAME_END_LEVEL = ConvertGameEvent((int)GameEvent.Type.EndLevel);

        public static readonly GameEvent EVENT_GAME_VARIABLE_LIMIT = ConvertGameEvent((int)GameEvent.Type.VariableLimit);
        public static readonly GameEvent EVENT_GAME_STATE_LIMIT = ConvertGameEvent((int)GameEvent.Type.StateLimit);

        public static readonly GameEvent EVENT_GAME_TIMER_EXPIRED = ConvertGameEvent((int)GameEvent.Type.TimerExpired);

        public static readonly GameEvent EVENT_GAME_ENTER_REGION = ConvertGameEvent((int)GameEvent.Type.EnterRegion);
        public static readonly GameEvent EVENT_GAME_LEAVE_REGION = ConvertGameEvent((int)GameEvent.Type.LeaveRegion);

        public static readonly GameEvent EVENT_GAME_TRACKABLE_HIT = ConvertGameEvent((int)GameEvent.Type.TrackableHit);
        public static readonly GameEvent EVENT_GAME_TRACKABLE_TRACK = ConvertGameEvent((int)GameEvent.Type.TrackableTrack);

        public static readonly GameEvent EVENT_GAME_SHOW_SKILL = ConvertGameEvent((int)GameEvent.Type.ShowSkill);
        public static readonly GameEvent EVENT_GAME_BUILD_SUBMENU = ConvertGameEvent((int)GameEvent.Type.BuildSubmenu);

        public static readonly GameEvent EVENT_GAME_LOADED = ConvertGameEvent((int)GameEvent.Type.Loaded);
        public static readonly GameEvent EVENT_GAME_TOURNAMENT_FINISH_SOON = ConvertGameEvent((int)GameEvent.Type.TournamentFinishSoon);
        public static readonly GameEvent EVENT_GAME_TOURNAMENT_FINISH_NOW = ConvertGameEvent((int)GameEvent.Type.TournamentFinishNow);
        public static readonly GameEvent EVENT_GAME_SAVE = ConvertGameEvent((int)GameEvent.Type.Save);
        public static readonly GameEvent EVENT_GAME_CUSTOM_UI_FRAME = ConvertGameEvent((int)GameEvent.Type.CustomUIFrame);

        public static GameEvent ConvertGameEvent(int i)
        {
            return GameEvent.GetGameEvent(i);
        }
    }
}