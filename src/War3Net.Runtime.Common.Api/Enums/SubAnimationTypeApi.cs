// ------------------------------------------------------------------------------
// <copyright file="SubAnimationTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
{
    public static class SubAnimationTypeApi
    {
        public static readonly SubAnimationType SUBANIM_TYPE_ROOTED = ConvertSubAnimType((int)SubAnimationType.Type.Rooted);
        public static readonly SubAnimationType SUBANIM_TYPE_ALTERNATE_EX = ConvertSubAnimType((int)SubAnimationType.Type.Alternate);
        public static readonly SubAnimationType SUBANIM_TYPE_LOOPING = ConvertSubAnimType((int)SubAnimationType.Type.Looping);
        public static readonly SubAnimationType SUBANIM_TYPE_SLAM = ConvertSubAnimType((int)SubAnimationType.Type.Slam);
        public static readonly SubAnimationType SUBANIM_TYPE_THROW = ConvertSubAnimType((int)SubAnimationType.Type.Throw);
        public static readonly SubAnimationType SUBANIM_TYPE_SPIKED = ConvertSubAnimType((int)SubAnimationType.Type.Spiked);
        public static readonly SubAnimationType SUBANIM_TYPE_FAST = ConvertSubAnimType((int)SubAnimationType.Type.Fast);
        public static readonly SubAnimationType SUBANIM_TYPE_SPIN = ConvertSubAnimType((int)SubAnimationType.Type.Spin);
        public static readonly SubAnimationType SUBANIM_TYPE_READY = ConvertSubAnimType((int)SubAnimationType.Type.Ready);
        public static readonly SubAnimationType SUBANIM_TYPE_CHANNEL = ConvertSubAnimType((int)SubAnimationType.Type.Channel);
        public static readonly SubAnimationType SUBANIM_TYPE_DEFEND = ConvertSubAnimType((int)SubAnimationType.Type.Defend);
        public static readonly SubAnimationType SUBANIM_TYPE_VICTORY = ConvertSubAnimType((int)SubAnimationType.Type.Victory);
        public static readonly SubAnimationType SUBANIM_TYPE_TURN = ConvertSubAnimType((int)SubAnimationType.Type.Turn);
        public static readonly SubAnimationType SUBANIM_TYPE_LEFT = ConvertSubAnimType((int)SubAnimationType.Type.Left);
        public static readonly SubAnimationType SUBANIM_TYPE_RIGHT = ConvertSubAnimType((int)SubAnimationType.Type.Right);
        public static readonly SubAnimationType SUBANIM_TYPE_FIRE = ConvertSubAnimType((int)SubAnimationType.Type.Fire);
        public static readonly SubAnimationType SUBANIM_TYPE_FLESH = ConvertSubAnimType((int)SubAnimationType.Type.Flesh);
        public static readonly SubAnimationType SUBANIM_TYPE_HIT = ConvertSubAnimType((int)SubAnimationType.Type.Hit);
        public static readonly SubAnimationType SUBANIM_TYPE_WOUNDED = ConvertSubAnimType((int)SubAnimationType.Type.Wounded);
        public static readonly SubAnimationType SUBANIM_TYPE_LIGHT = ConvertSubAnimType((int)SubAnimationType.Type.Light);
        public static readonly SubAnimationType SUBANIM_TYPE_MODERATE = ConvertSubAnimType((int)SubAnimationType.Type.Moderate);
        public static readonly SubAnimationType SUBANIM_TYPE_SEVERE = ConvertSubAnimType((int)SubAnimationType.Type.Severe);
        public static readonly SubAnimationType SUBANIM_TYPE_CRITICAL = ConvertSubAnimType((int)SubAnimationType.Type.Critical);
        public static readonly SubAnimationType SUBANIM_TYPE_COMPLETE = ConvertSubAnimType((int)SubAnimationType.Type.Complete);
        public static readonly SubAnimationType SUBANIM_TYPE_GOLD = ConvertSubAnimType((int)SubAnimationType.Type.Gold);
        public static readonly SubAnimationType SUBANIM_TYPE_LUMBER = ConvertSubAnimType((int)SubAnimationType.Type.Lumber);
        public static readonly SubAnimationType SUBANIM_TYPE_WORK = ConvertSubAnimType((int)SubAnimationType.Type.Work);
        public static readonly SubAnimationType SUBANIM_TYPE_TALK = ConvertSubAnimType((int)SubAnimationType.Type.Talk);
        public static readonly SubAnimationType SUBANIM_TYPE_FIRST = ConvertSubAnimType((int)SubAnimationType.Type.First);
        public static readonly SubAnimationType SUBANIM_TYPE_SECOND = ConvertSubAnimType((int)SubAnimationType.Type.Second);
        public static readonly SubAnimationType SUBANIM_TYPE_THIRD = ConvertSubAnimType((int)SubAnimationType.Type.Third);
        public static readonly SubAnimationType SUBANIM_TYPE_FOURTH = ConvertSubAnimType((int)SubAnimationType.Type.Fourth);
        public static readonly SubAnimationType SUBANIM_TYPE_FIFTH = ConvertSubAnimType((int)SubAnimationType.Type.Fifth);
        public static readonly SubAnimationType SUBANIM_TYPE_ONE = ConvertSubAnimType((int)SubAnimationType.Type.One);
        public static readonly SubAnimationType SUBANIM_TYPE_TWO = ConvertSubAnimType((int)SubAnimationType.Type.Two);
        public static readonly SubAnimationType SUBANIM_TYPE_THREE = ConvertSubAnimType((int)SubAnimationType.Type.Three);
        public static readonly SubAnimationType SUBANIM_TYPE_FOUR = ConvertSubAnimType((int)SubAnimationType.Type.Four);
        public static readonly SubAnimationType SUBANIM_TYPE_FIVE = ConvertSubAnimType((int)SubAnimationType.Type.Five);
        public static readonly SubAnimationType SUBANIM_TYPE_SMALL = ConvertSubAnimType((int)SubAnimationType.Type.Small);
        public static readonly SubAnimationType SUBANIM_TYPE_MEDIUM = ConvertSubAnimType((int)SubAnimationType.Type.Medium);
        public static readonly SubAnimationType SUBANIM_TYPE_LARGE = ConvertSubAnimType((int)SubAnimationType.Type.Large);
        public static readonly SubAnimationType SUBANIM_TYPE_UPGRADE = ConvertSubAnimType((int)SubAnimationType.Type.Upgrade);
        public static readonly SubAnimationType SUBANIM_TYPE_DRAIN = ConvertSubAnimType((int)SubAnimationType.Type.Drain);
        public static readonly SubAnimationType SUBANIM_TYPE_FILL = ConvertSubAnimType((int)SubAnimationType.Type.Fill);
        public static readonly SubAnimationType SUBANIM_TYPE_CHAINLIGHTNING = ConvertSubAnimType((int)SubAnimationType.Type.ChainLightning);
        public static readonly SubAnimationType SUBANIM_TYPE_EATTREE = ConvertSubAnimType((int)SubAnimationType.Type.EatTree);
        public static readonly SubAnimationType SUBANIM_TYPE_PUKE = ConvertSubAnimType((int)SubAnimationType.Type.Puke);
        public static readonly SubAnimationType SUBANIM_TYPE_FLAIL = ConvertSubAnimType((int)SubAnimationType.Type.Flail);
        public static readonly SubAnimationType SUBANIM_TYPE_OFF = ConvertSubAnimType((int)SubAnimationType.Type.Off);
        public static readonly SubAnimationType SUBANIM_TYPE_SWIM = ConvertSubAnimType((int)SubAnimationType.Type.Swim);
        public static readonly SubAnimationType SUBANIM_TYPE_ENTANGLE = ConvertSubAnimType((int)SubAnimationType.Type.Entangle);
        public static readonly SubAnimationType SUBANIM_TYPE_BERSERK = ConvertSubAnimType((int)SubAnimationType.Type.Berserk);

        public static SubAnimationType ConvertSubAnimType(int i)
        {
            return SubAnimationType.GetSubAnimationType(i);
        }
    }
}