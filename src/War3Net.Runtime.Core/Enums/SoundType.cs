using System;
using System.Collections.Generic;
using System.Linq;
using War3Net.Runtime.Core;

namespace War3Net.Runtime.Enums
{
    public sealed class SoundType : Handle
    {
        private static readonly Dictionary<int, SoundType> _types = GetTypes().ToDictionary(t => (int)t, t => new SoundType(t));

        private readonly Type _type;

        private SoundType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Effect = 0,
            EffectLooped = 1,
        }

        public static implicit operator Type(SoundType soundType) => soundType._type;

        public static explicit operator int(SoundType soundType) => (int)soundType._type;

        public static SoundType GetSoundType(int i)
        {
            if (!_types.TryGetValue(i, out var soundType))
            {
                soundType = new SoundType((Type)i);
                _types.Add(i, soundType);
            }

            return soundType;
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