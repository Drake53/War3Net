// ------------------------------------------------------------------------------
// <copyright file="UnitEvent.cs" company="Drake53">
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
    public sealed class UnitEvent : EventId
    {
        private static readonly Dictionary<int, UnitEvent> _events = GetTypes().ToDictionary(t => (int)t, t => new UnitEvent(t));

        private readonly Type _type;

        private UnitEvent(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Damaged = 52,
            Damaging = 314,

            Death = 53,
            Decay = 54,

            Detected = 55,
            Hidden = 56,

            Selected = 57,
            Deselected = 58,

            StateLimit = 59,

            AcquiredTarget = 60,
            TargetInRange = 61,

            Attacked = 62,
            Rescued = 63,

            ConstructCancel = 64,
            ConstructFinish = 65,

            UpgradeStart = 66,
            UpgradeCancel = 67,
            UpgradeFinish = 68,

            TrainStart = 69,
            TrainCancel = 70,
            TrainFinish = 71,

            ResearchStart = 72,
            ResearchCancel = 73,
            ResearchFinish = 74,

            IssuedOrder = 75,
            IssuedPointOrder = 76,
            IssuedTargetOrder = 77,

            HeroLevel = 78,
            HeroSkill = 79,

            HeroRevivable = 80,
            HeroReviveStart = 81,
            HeroReviveCancel = 82,
            HeroReviveFinish = 83,

            Summon = 84,

            DropItem = 85,
            PickupItem = 86,
            UseItem = 87,

            Loaded = 88,

            Sell = 286,
            ChangeOwner = 287,
            SellItem = 288,

            SpellChannel = 289,
            SpellCast = 290,
            SpellEffect = 291,
            SpellFinish = 292,
            SpellEndcast = 293,

            PawnItem = 294,
        }

        public static UnitEvent GetUnitEvent(int i)
        {
            if (!_events.TryGetValue(i, out var unitEvent))
            {
                unitEvent = new UnitEvent((Type)i);
                _events.Add(i, unitEvent);
            }

            return unitEvent;
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