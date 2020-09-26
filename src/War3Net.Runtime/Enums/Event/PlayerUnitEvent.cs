// ------------------------------------------------------------------------------
// <copyright file="PlayerUnitEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums.Event
{
    public sealed class PlayerUnitEvent : EventId
    {
        private static readonly Dictionary<int, PlayerUnitEvent> _events = GetTypes().ToDictionary(t => (int)t, t => new PlayerUnitEvent(t));

        private readonly Type _type;

        private PlayerUnitEvent(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Attacked = 18,
            Rescued = 19,

            Death = 20,
            Decay = 21,

            Detected = 22,
            Hidden = 23,

            Selected = 24,
            Deselected = 25,

            ConstructStart = 26,
            ConstructCancel = 27,
            ConstructFinish = 28,

            UpgradeStart = 29,
            UpgradeCancel = 30,
            UpgradeFinish = 31,

            TrainStart = 32,
            TrainCancel = 33,
            TrainFinish = 34,

            ResearchStart = 35,
            ResearchCancel = 36,
            ResearchFinish = 37,

            IssuedOrder = 38,
            IssuedPointOrder = 39,
            IssuedTargetOrder = 40,

            HeroLevel = 41,
            HeroSkill = 42,

            HeroRevivable = 43,
            HeroReviveStart = 44,
            HeroReviveCancel = 45,
            HeroReviveFinish = 46,

            Summon = 47,

            DropItem = 48,
            PickupItem = 49,
            UseItem = 50,

            Loaded = 51,

            Sell = 269,
            ChangeOwner = 270,
            SellItem = 271,

            SpellChannel = 272,
            SpellCast = 273,
            SpellEffect = 274,
            SpellFinish = 275,
            SpellEndcast = 276,

            PawnItem = 277,

            Damaged = 308,
            Damaging = 315,
        }

        public static PlayerUnitEvent GetPlayerUnitEvent(int i)
        {
            if (!_events.TryGetValue(i, out var playerUnitEvent))
            {
                playerUnitEvent = new PlayerUnitEvent((Type)i);
                _events.Add(i, playerUnitEvent);
            }

            return playerUnitEvent;
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