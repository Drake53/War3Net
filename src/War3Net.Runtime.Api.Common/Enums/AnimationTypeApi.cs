// ------------------------------------------------------------------------------
// <copyright file="AnimationTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class AnimationTypeApi
    {
        public static readonly AnimationType ANIM_TYPE_BIRTH = ConvertAnimType((int)AnimationType.Type.Birth);
        public static readonly AnimationType ANIM_TYPE_DEATH = ConvertAnimType((int)AnimationType.Type.Death);
        public static readonly AnimationType ANIM_TYPE_DECAY = ConvertAnimType((int)AnimationType.Type.Decay);
        public static readonly AnimationType ANIM_TYPE_DISSIPATE = ConvertAnimType((int)AnimationType.Type.Dissipate);
        public static readonly AnimationType ANIM_TYPE_STAND = ConvertAnimType((int)AnimationType.Type.Stand);
        public static readonly AnimationType ANIM_TYPE_WALK = ConvertAnimType((int)AnimationType.Type.Walk);
        public static readonly AnimationType ANIM_TYPE_ATTACK = ConvertAnimType((int)AnimationType.Type.Attack);
        public static readonly AnimationType ANIM_TYPE_MORPH = ConvertAnimType((int)AnimationType.Type.Morph);
        public static readonly AnimationType ANIM_TYPE_SLEEP = ConvertAnimType((int)AnimationType.Type.Sleep);
        public static readonly AnimationType ANIM_TYPE_SPELL = ConvertAnimType((int)AnimationType.Type.Spell);
        public static readonly AnimationType ANIM_TYPE_PORTRAIT = ConvertAnimType((int)AnimationType.Type.Portrait);

        public static AnimationType ConvertAnimType(int i)
        {
            return AnimationType.GetAnimationType(i);
        }
    }
}