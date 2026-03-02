#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class SoundTypeApi
    {
        public static readonly SoundType SOUND_TYPE_EFFECT = ConvertSoundType((int)SoundType.Type.Effect);
        public static readonly SoundType SOUND_TYPE_EFFECT_LOOPED = ConvertSoundType((int)SoundType.Type.EffectLooped);

        public static SoundType ConvertSoundType(int i)
        {
            return SoundType.GetSoundType(i);
        }
    }
}