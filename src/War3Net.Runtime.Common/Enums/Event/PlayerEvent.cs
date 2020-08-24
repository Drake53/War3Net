// ------------------------------------------------------------------------------
// <copyright file="PlayerEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Event
{
    public sealed class PlayerEvent : EventId
    {
        private static readonly Dictionary<int, PlayerEvent> _events = GetTypes().ToDictionary(t => (int)t, t => new PlayerEvent(t));

        private readonly Type _type;

        private PlayerEvent(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            StateLimit = 11,
            AllianceChanged = 12,

            Defeat = 13,
            Victory = 14,
            Leave = 15,
            Chat = 16,
            EndCinematic = 17,

            ArrowLeftDown = 261,
            ArrowLeftUp = 262,
            ArrowRightDown = 263,
            ArrowRightUp = 264,
            ArrowDownDown = 265,
            ArrowDownUp = 266,
            ArrowUpDown = 267,
            ArrowUpUp = 268,

            MouseDown = 305,
            MouseUp = 306,
            MouseMove = 307,

            SyncData = 309,

            Key = 311,
            KeyDown = 312,
            KeyUp = 313,
        }

        public static PlayerEvent? GetPlayerEvent(int i)
        {
            return _events.TryGetValue(i, out var playerEvent) ? playerEvent : null;
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