// ------------------------------------------------------------------------------
// <copyright file="UnitEventApi.cs" company="Drake53">
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
    public static class UnitEventApi
    {
        public static readonly UnitEvent EVENT_UNIT_DAMAGED = ConvertUnitEvent((int)UnitEvent.Type.Damaged);
        public static readonly UnitEvent EVENT_UNIT_DAMAGING = ConvertUnitEvent((int)UnitEvent.Type.Damaging);
        public static readonly UnitEvent EVENT_UNIT_DEATH = ConvertUnitEvent((int)UnitEvent.Type.Death);
        public static readonly UnitEvent EVENT_UNIT_DECAY = ConvertUnitEvent((int)UnitEvent.Type.Decay);
        public static readonly UnitEvent EVENT_UNIT_DETECTED = ConvertUnitEvent((int)UnitEvent.Type.Detected);
        public static readonly UnitEvent EVENT_UNIT_HIDDEN = ConvertUnitEvent((int)UnitEvent.Type.Hidden);
        public static readonly UnitEvent EVENT_UNIT_SELECTED = ConvertUnitEvent((int)UnitEvent.Type.Selected);
        public static readonly UnitEvent EVENT_UNIT_DESELECTED = ConvertUnitEvent((int)UnitEvent.Type.Deselected);

        public static readonly UnitEvent EVENT_UNIT_STATE_LIMIT = ConvertUnitEvent((int)UnitEvent.Type.StateLimit);

        public static readonly UnitEvent EVENT_UNIT_ACQUIRED_TARGET = ConvertUnitEvent((int)UnitEvent.Type.AcquiredTarget);
        public static readonly UnitEvent EVENT_UNIT_TARGET_IN_RANGE = ConvertUnitEvent((int)UnitEvent.Type.TargetInRange);
        public static readonly UnitEvent EVENT_UNIT_ATTACKED = ConvertUnitEvent((int)UnitEvent.Type.Attacked);
        public static readonly UnitEvent EVENT_UNIT_RESCUED = ConvertUnitEvent((int)UnitEvent.Type.Rescued);

        public static readonly UnitEvent EVENT_UNIT_CONSTRUCT_CANCEL = ConvertUnitEvent((int)UnitEvent.Type.ConstructCancel);
        public static readonly UnitEvent EVENT_UNIT_CONSTRUCT_FINISH = ConvertUnitEvent((int)UnitEvent.Type.ConstructFinish);

        public static readonly UnitEvent EVENT_UNIT_UPGRADE_START = ConvertUnitEvent((int)UnitEvent.Type.UpgradeStart);
        public static readonly UnitEvent EVENT_UNIT_UPGRADE_CANCEL = ConvertUnitEvent((int)UnitEvent.Type.UpgradeCancel);
        public static readonly UnitEvent EVENT_UNIT_UPGRADE_FINISH = ConvertUnitEvent((int)UnitEvent.Type.UpgradeFinish);

        public static readonly UnitEvent EVENT_UNIT_TRAIN_START = ConvertUnitEvent((int)UnitEvent.Type.TrainStart);
        public static readonly UnitEvent EVENT_UNIT_TRAIN_CANCEL = ConvertUnitEvent((int)UnitEvent.Type.TrainCancel);
        public static readonly UnitEvent EVENT_UNIT_TRAIN_FINISH = ConvertUnitEvent((int)UnitEvent.Type.TrainFinish);

        public static readonly UnitEvent EVENT_UNIT_RESEARCH_START = ConvertUnitEvent((int)UnitEvent.Type.ResearchStart);
        public static readonly UnitEvent EVENT_UNIT_RESEARCH_CANCEL = ConvertUnitEvent((int)UnitEvent.Type.ResearchCancel);
        public static readonly UnitEvent EVENT_UNIT_RESEARCH_FINISH = ConvertUnitEvent((int)UnitEvent.Type.ResearchFinish);

        public static readonly UnitEvent EVENT_UNIT_ISSUED_ORDER = ConvertUnitEvent((int)UnitEvent.Type.IssuedOrder);
        public static readonly UnitEvent EVENT_UNIT_ISSUED_POINT_ORDER = ConvertUnitEvent((int)UnitEvent.Type.IssuedPointOrder);
        public static readonly UnitEvent EVENT_UNIT_ISSUED_TARGET_ORDER = ConvertUnitEvent((int)UnitEvent.Type.IssuedTargetOrder);

        public static readonly UnitEvent EVENT_UNIT_HERO_LEVEL = ConvertUnitEvent((int)UnitEvent.Type.HeroLevel);
        public static readonly UnitEvent EVENT_UNIT_HERO_SKILL = ConvertUnitEvent((int)UnitEvent.Type.HeroSkill);

        public static readonly UnitEvent EVENT_UNIT_HERO_REVIVABLE = ConvertUnitEvent((int)UnitEvent.Type.HeroRevivable);
        public static readonly UnitEvent EVENT_UNIT_HERO_REVIVE_START = ConvertUnitEvent((int)UnitEvent.Type.HeroReviveStart);
        public static readonly UnitEvent EVENT_UNIT_HERO_REVIVE_CANCEL = ConvertUnitEvent((int)UnitEvent.Type.HeroReviveCancel);
        public static readonly UnitEvent EVENT_UNIT_HERO_REVIVE_FINISH = ConvertUnitEvent((int)UnitEvent.Type.HeroReviveFinish);

        public static readonly UnitEvent EVENT_UNIT_SUMMON = ConvertUnitEvent((int)UnitEvent.Type.Summon);

        public static readonly UnitEvent EVENT_UNIT_DROP_ITEM = ConvertUnitEvent((int)UnitEvent.Type.DropItem);
        public static readonly UnitEvent EVENT_UNIT_PICKUP_ITEM = ConvertUnitEvent((int)UnitEvent.Type.PickupItem);
        public static readonly UnitEvent EVENT_UNIT_USE_ITEM = ConvertUnitEvent((int)UnitEvent.Type.UseItem);

        public static readonly UnitEvent EVENT_UNIT_LOADED = ConvertUnitEvent((int)UnitEvent.Type.Loaded);

        public static readonly UnitEvent EVENT_UNIT_SELL = ConvertUnitEvent((int)UnitEvent.Type.Sell);
        public static readonly UnitEvent EVENT_UNIT_CHANGE_OWNER = ConvertUnitEvent((int)UnitEvent.Type.ChangeOwner);
        public static readonly UnitEvent EVENT_UNIT_SELL_ITEM = ConvertUnitEvent((int)UnitEvent.Type.SellItem);
        public static readonly UnitEvent EVENT_UNIT_SPELL_CHANNEL = ConvertUnitEvent((int)UnitEvent.Type.SpellChannel);
        public static readonly UnitEvent EVENT_UNIT_SPELL_CAST = ConvertUnitEvent((int)UnitEvent.Type.SpellCast);
        public static readonly UnitEvent EVENT_UNIT_SPELL_EFFECT = ConvertUnitEvent((int)UnitEvent.Type.SpellEffect);
        public static readonly UnitEvent EVENT_UNIT_SPELL_FINISH = ConvertUnitEvent((int)UnitEvent.Type.SpellFinish);
        public static readonly UnitEvent EVENT_UNIT_SPELL_ENDCAST = ConvertUnitEvent((int)UnitEvent.Type.SpellEndcast);
        public static readonly UnitEvent EVENT_UNIT_PAWN_ITEM = ConvertUnitEvent((int)UnitEvent.Type.PawnItem);

        public static UnitEvent ConvertUnitEvent(int i)
        {
            return UnitEvent.GetUnitEvent(i);
        }
    }
}