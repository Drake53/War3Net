// ------------------------------------------------------------------------------
// <copyright file="PlayerUnitEventApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using System;

using War3Net.Runtime.Common.Enums.Event;

namespace War3Net.Runtime.Common.Api.Enums.Event
{
    public static class PlayerUnitEventApi
    {
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_ATTACKED = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Attacked);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_RESCUED = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Rescued);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_DEATH = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Death);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_DECAY = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Decay);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_DETECTED = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Detected);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_HIDDEN = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Hidden);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_SELECTED = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Selected);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_DESELECTED = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Deselected);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_CONSTRUCT_START = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.ConstructStart);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_CONSTRUCT_CANCEL = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.ConstructCancel);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_CONSTRUCT_FINISH = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.ConstructFinish);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_UPGRADE_START = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.UpgradeStart);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_UPGRADE_CANCEL = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.UpgradeCancel);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_UPGRADE_FINISH = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.UpgradeFinish);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_TRAIN_START = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.TrainStart);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_TRAIN_CANCEL = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.TrainCancel);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_TRAIN_FINISH = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.TrainFinish);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_RESEARCH_START = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.ResearchStart);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_RESEARCH_CANCEL = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.ResearchCancel);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_RESEARCH_FINISH = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.ResearchFinish);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_ISSUED_ORDER = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.IssuedOrder);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_ISSUED_POINT_ORDER = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.IssuedPointOrder);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_ISSUED_TARGET_ORDER = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.IssuedTargetOrder);

        [Obsolete("Use " + nameof(EVENT_PLAYER_UNIT_ISSUED_TARGET_ORDER) + " instead")]
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_ISSUED_UNIT_ORDER = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.IssuedTargetOrder);

        public static readonly PlayerUnitEvent EVENT_PLAYER_HERO_LEVEL = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.HeroLevel);
        public static readonly PlayerUnitEvent EVENT_PLAYER_HERO_SKILL = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.HeroSkill);

        public static readonly PlayerUnitEvent EVENT_PLAYER_HERO_REVIVABLE = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.HeroRevivable);

        public static readonly PlayerUnitEvent EVENT_PLAYER_HERO_REVIVE_START = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.HeroReviveStart);
        public static readonly PlayerUnitEvent EVENT_PLAYER_HERO_REVIVE_CANCEL = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.HeroReviveCancel);
        public static readonly PlayerUnitEvent EVENT_PLAYER_HERO_REVIVE_FINISH = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.HeroReviveFinish);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_SUMMON = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Summon);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_DROP_ITEM = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.DropItem);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_PICKUP_ITEM = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.PickupItem);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_USE_ITEM = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.UseItem);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_LOADED = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Loaded);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_DAMAGED = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Damaged);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_DAMAGING = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Damaging);

        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_SELL = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.Sell);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_CHANGE_OWNER = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.ChangeOwner);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_SELL_ITEM = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.SellItem);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_SPELL_CHANNEL = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.SpellChannel);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_SPELL_CAST = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.SpellCast);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_SPELL_EFFECT = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.SpellEffect);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_SPELL_FINISH = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.SpellFinish);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_SPELL_ENDCAST = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.SpellEndcast);
        public static readonly PlayerUnitEvent EVENT_PLAYER_UNIT_PAWN_ITEM = ConvertPlayerUnitEvent((int)PlayerUnitEvent.Type.PawnItem);

        public static PlayerUnitEvent ConvertPlayerUnitEvent(int i)
        {
            return PlayerUnitEvent.GetPlayerUnitEvent(i);
        }
    }
}