// ------------------------------------------------------------------------------
// <copyright file="GameEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Event
{
    public sealed class GameEvent : EventId
    {
        private static readonly Dictionary<int, GameEvent> _events = GetTypes().ToDictionary(t => (int)t, t => new GameEvent(t));

        private readonly Type _type;

        private GameEvent(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Victory = 0,
            EndLevel = 1,

            VariableLimit = 2,
            StateLimit = 3,

            TimerExpired = 4,

            EnterRegion = 5,
            LeaveRegion = 6,

            TrackableHit = 7,
            TrackableTrack = 8,

            ShowSkill = 9,
            BuildSubmenu = 10,

            Loaded = 256,

            TournamentFinishSoon = 257,
            TournamentFinishNow = 258,

            Save = 259,

            CustomUIFrame = 310,
        }

        public static GameEvent GetGameEvent(int i)
        {
            if (!_events.TryGetValue(i, out var gameEvent))
            {
                gameEvent = new GameEvent((Type)i);
                _events.Add(i, gameEvent);
            }

            return gameEvent;
        }

        private static IEnumerable<Type> GetTypes()
        {
            foreach (Type type in Enum.GetValues(typeof(Type)))
            {
                yield return type;
            }
        }
    }
}