// ------------------------------------------------------------------------------
// <copyright file="UnitEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace War3Net.Runtime.Api.Enums
{
    public class UnitEvent : EventId
    {
        /// @CSharpLua.Template = "EVENT_UNIT_DAMAGED"
        public static readonly UnitEvent Damaged;

        /// @CSharpLua.Template = "EVENT_UNIT_DAMAGING"
        public static readonly UnitEvent Damaging;

        /// @CSharpLua.Template = "EVENT_UNIT_DEATH"
        public static readonly UnitEvent Death;

        /// @CSharpLua.Template = "EVENT_UNIT_DECAY"
        public static readonly UnitEvent Decay;

        /// @CSharpLua.Template = "EVENT_UNIT_DETECTED"
        public static readonly UnitEvent Detected;

        /// @CSharpLua.Template = "EVENT_UNIT_HIDDEN"
        public static readonly UnitEvent Hidden;

        /// @CSharpLua.Template = "EVENT_UNIT_SELECTED"
        public static readonly UnitEvent Selected;

        /// @CSharpLua.Template = "EVENT_UNIT_DESELECTED"
        public static readonly UnitEvent Deselected;

        /// @CSharpLua.Template = "EVENT_UNIT_STATE_LIMIT"
        public static readonly UnitEvent StateLimit;

        /// @CSharpLua.Template = "EVENT_UNIT_ACQUIRED_TARGET"
        public static readonly UnitEvent AcquiredTarget;

        /// @CSharpLua.Template = "EVENT_UNIT_TARGET_IN_RANGE"
        public static readonly UnitEvent TargetInRange;

        /// @CSharpLua.Template = "EVENT_UNIT_ATTACKED"
        public static readonly UnitEvent Attacked;

        /// @CSharpLua.Template = "EVENT_UNIT_RESCUED"
        public static readonly UnitEvent Rescued;

        /// @CSharpLua.Template = "EVENT_UNIT_CONSTRUCT_CANCEL"
        public static readonly UnitEvent ConstructCancel;

        /// @CSharpLua.Template = "EVENT_UNIT_CONSTRUCT_FINISH"
        public static readonly UnitEvent ConstructFinish;

        /// @CSharpLua.Template = "EVENT_UNIT_UPGRADE_START"
        public static readonly UnitEvent UpgradeStart;

        /// @CSharpLua.Template = "EVENT_UNIT_UPGRADE_CANCEL"
        public static readonly UnitEvent UpgradeCancel;

        /// @CSharpLua.Template = "EVENT_UNIT_UPGRADE_FINISH"
        public static readonly UnitEvent UpgradeFinish;

        /// @CSharpLua.Template = "EVENT_UNIT_TRAIN_START"
        public static readonly UnitEvent TrainStart;

        /// @CSharpLua.Template = "EVENT_UNIT_TRAIN_CANCEL"
        public static readonly UnitEvent TrainCancel;

        /// @CSharpLua.Template = "EVENT_UNIT_TRAIN_FINISH"
        public static readonly UnitEvent TrainFinish;

        /// @CSharpLua.Template = "EVENT_UNIT_RESEARCH_START"
        public static readonly UnitEvent ResearchStart;

        /// @CSharpLua.Template = "EVENT_UNIT_RESEARCH_CANCEL"
        public static readonly UnitEvent ResearchCancel;

        /// @CSharpLua.Template = "EVENT_UNIT_RESEARCH_FINISH"
        public static readonly UnitEvent ResearchFinish;

        /// @CSharpLua.Template = "EVENT_UNIT_ISSUED_ORDER"
        public static readonly UnitEvent IssuedOrder;

        /// @CSharpLua.Template = "EVENT_UNIT_ISSUED_POINT_ORDER"
        public static readonly UnitEvent IssuedPointOrder;

        /// @CSharpLua.Template = "EVENT_UNIT_ISSUED_TARGET_ORDER"
        public static readonly UnitEvent IssuedTargetOrder;

        /// @CSharpLua.Template = "EVENT_UNIT_HERO_LEVEL"
        public static readonly UnitEvent HeroLevel;

        /// @CSharpLua.Template = "EVENT_UNIT_HERO_SKILL"
        public static readonly UnitEvent HeroSkill;

        /// @CSharpLua.Template = "EVENT_UNIT_HERO_REVIVABLE"
        public static readonly UnitEvent HeroRevivable;

        /// @CSharpLua.Template = "EVENT_UNIT_HERO_REVIVE_START"
        public static readonly UnitEvent HeroReviveStart;

        /// @CSharpLua.Template = "EVENT_UNIT_HERO_REVIVE_CANCEL"
        public static readonly UnitEvent HeroReviveCancel;

        /// @CSharpLua.Template = "EVENT_UNIT_HERO_REVIVE_FINISH"
        public static readonly UnitEvent HeroReviveFinish;

        /// @CSharpLua.Template = "EVENT_UNIT_SUMMON"
        public static readonly UnitEvent Summon;

        /// @CSharpLua.Template = "EVENT_UNIT_DROP_ITEM"
        public static readonly UnitEvent DropItem;

        /// @CSharpLua.Template = "EVENT_UNIT_PICKUP_ITEM"
        public static readonly UnitEvent PickUpItem;

        /// @CSharpLua.Template = "EVENT_UNIT_USE_ITEM"
        public static readonly UnitEvent UseItem;

        /// @CSharpLua.Template = "EVENT_UNIT_LOADED"
        public static readonly UnitEvent Loaded;

        /// @CSharpLua.Template = "EVENT_UNIT_SELL"
        public static readonly UnitEvent Sell;

        /// @CSharpLua.Template = "EVENT_UNIT_CHANGE_OWNER"
        public static readonly UnitEvent ChangeOwner;

        /// @CSharpLua.Template = "EVENT_UNIT_SELL_ITEM"
        public static readonly UnitEvent SellItem;

        /// @CSharpLua.Template = "EVENT_UNIT_SPELL_CHANNEL"
        public static readonly UnitEvent SpellChannel;

        /// @CSharpLua.Template = "EVENT_UNIT_SPELL_CAST"
        public static readonly UnitEvent SpellCast;

        /// @CSharpLua.Template = "EVENT_UNIT_SPELL_EFFECT"
        public static readonly UnitEvent SpellEffect;

        /// @CSharpLua.Template = "EVENT_UNIT_SPELL_FINISH"
        public static readonly UnitEvent SpellFinish;

        /// @CSharpLua.Template = "EVENT_UNIT_SPELL_ENDCAST"
        public static readonly UnitEvent SpellEndCast;

        /// @CSharpLua.Template = "EVENT_UNIT_PAWN_ITEM"
        public static readonly UnitEvent PawnItem;

        /// @CSharpLua.Template = "ConvertUnitEvent({0})"
        public extern UnitEvent(int id);

        /// @CSharpLua.Template = "ConvertUnitEvent({0})"
        public static extern UnitEvent GetById(int id);
    }
}