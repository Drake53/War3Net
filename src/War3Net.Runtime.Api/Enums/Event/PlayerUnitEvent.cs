// ------------------------------------------------------------------------------
// <copyright file="PlayerUnitEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace War3Net.Runtime.Api.Enums
{
    public class PlayerUnitEvent : EventId
    {
        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_ATTACKED"
        public static readonly PlayerUnitEvent Attacked;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_RESCUED"
        public static readonly PlayerUnitEvent Rescued;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_DEATH"
        public static readonly PlayerUnitEvent Death;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_DECAY"
        public static readonly PlayerUnitEvent Decay;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_DETECTED"
        public static readonly PlayerUnitEvent Detected;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_HIDDEN"
        public static readonly PlayerUnitEvent Hidden;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_SELECTED"
        public static readonly PlayerUnitEvent Selected;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_DESELECTED"
        public static readonly PlayerUnitEvent Deselected;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_CONSTRUCT_START"
        public static readonly PlayerUnitEvent ConstructStart;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_CONSTRUCT_CANCEL"
        public static readonly PlayerUnitEvent ConstructCancel;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_CONSTRUCT_FINISH"
        public static readonly PlayerUnitEvent ConstructFinish;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_UPGRADE_START"
        public static readonly PlayerUnitEvent UpgradeStart;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_UPGRADE_CANCEL"
        public static readonly PlayerUnitEvent UpgradeCancel;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_UPGRADE_FINISH"
        public static readonly PlayerUnitEvent UpgradeFinish;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_TRAIN_START"
        public static readonly PlayerUnitEvent TrainStart;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_TRAIN_CANCEL"
        public static readonly PlayerUnitEvent TrainCancel;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_TRAIN_FINISH"
        public static readonly PlayerUnitEvent TrainFinish;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_RESEARCH_START"
        public static readonly PlayerUnitEvent ResearchStart;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_RESEARCH_CANCEL"
        public static readonly PlayerUnitEvent ResearchCancel;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_RESEARCH_FINISH"
        public static readonly PlayerUnitEvent ResearchFinish;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_ISSUED_ORDER"
        public static readonly PlayerUnitEvent IssuedOrder;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_ISSUED_POINT_ORDER"
        public static readonly PlayerUnitEvent IssuedPointOrder;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_ISSUED_TARGET_ORDER"
        public static readonly PlayerUnitEvent IssuedTargetOrder;

        /// @CSharpLua.Template = "EVENT_PLAYER_HERO_LEVEL"
        public static readonly PlayerUnitEvent HeroLevel;

        /// @CSharpLua.Template = "EVENT_PLAYER_HERO_SKILL"
        public static readonly PlayerUnitEvent HeroSkill;

        /// @CSharpLua.Template = "EVENT_PLAYER_HERO_REVIVABLE"
        public static readonly PlayerUnitEvent HeroRevivable;

        /// @CSharpLua.Template = "EVENT_PLAYER_HERO_REVIVE_START"
        public static readonly PlayerUnitEvent HeroReviveStart;

        /// @CSharpLua.Template = "EVENT_PLAYER_HERO_REVIVE_CANCEL"
        public static readonly PlayerUnitEvent HeroReviveCancel;

        /// @CSharpLua.Template = "EVENT_PLAYER_HERO_REVIVE_FINISH"
        public static readonly PlayerUnitEvent HeroReviveFinish;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_SUMMON"
        public static readonly PlayerUnitEvent Summon;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_DROP_ITEM"
        public static readonly PlayerUnitEvent DropItem;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_PICKUP_ITEM"
        public static readonly PlayerUnitEvent PickUpItem;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_USE_ITEM"
        public static readonly PlayerUnitEvent UseItem;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_LOADED"
        public static readonly PlayerUnitEvent Loaded;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_DAMAGED"
        public static readonly PlayerUnitEvent Damaged;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_DAMAGING"
        public static readonly PlayerUnitEvent Damaging;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_SELL"
        public static readonly PlayerUnitEvent Sell;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_CHANGE_OWNER"
        public static readonly PlayerUnitEvent ChangeOwner;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_SELL_ITEM"
        public static readonly PlayerUnitEvent SellItem;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_SPELL_CHANNEL"
        public static readonly PlayerUnitEvent SpellChannel;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_SPELL_CAST"
        public static readonly PlayerUnitEvent SpellCast;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_SPELL_EFFECT"
        public static readonly PlayerUnitEvent SpellEffect;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_SPELL_FINISH"
        public static readonly PlayerUnitEvent SpellFinish;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_SPELL_ENDCAST"
        public static readonly PlayerUnitEvent SpellEndCast;

        /// @CSharpLua.Template = "EVENT_PLAYER_UNIT_PAWN_ITEM"
        public static readonly PlayerUnitEvent PawnItem;

        /// @CSharpLua.Template = "ConvertPlayerUnitEvent({0})"
        public extern PlayerUnitEvent(int id);

        /// @CSharpLua.Template = "ConvertPlayerUnitEvent({0})"
        public static extern PlayerUnitEvent GetById(int id);
    }
}