// ------------------------------------------------------------------------------
// <copyright file="SubAnimationType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.Enums
{
    public sealed class SubAnimationType : Handle
    {
        private static readonly Dictionary<int, SubAnimationType> _types = GetTypes().ToDictionary(t => (int)t, t => new SubAnimationType(t));

        private readonly Type _type;

        private SubAnimationType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Rooted = 11,
            Alternate = 12,
            Looping = 13,
            Slam = 14,
            Throw = 15,
            Spiked = 16,
            Fast = 17,
            Spin = 18,
            Ready = 19,
            Channel = 20,
            Defend = 21,
            Victory = 22,
            Turn = 23,
            Left = 24,
            Right = 25,
            Fire = 26,
            Flesh = 27,
            Hit = 28,
            Wounded = 29,
            Light = 30,
            Moderate = 31,
            Severe = 32,
            Critical = 33,
            Complete = 34,
            Gold = 35,
            Lumber= 36,
            Work = 37,
            Talk = 38,
            First = 39,
            Second = 40,
            Third = 41,
            Fourth = 42,
            Fifth = 43,
            One = 44,
            Two = 45,
            Three = 46,
            Four = 47,
            Five = 48,
            Small = 49,
            Medium = 50,
            Large = 51,
            Upgrade = 52,
            Drain = 53,
            Fill = 54,
            ChainLightning = 55,
            EatTree = 56,
            Puke = 57,
            Flail = 58,
            Off = 59,
            Swim = 60,
            Entangle = 61,
            Berserk = 62,
        }

        public static implicit operator Type(SubAnimationType subAnimationType) => subAnimationType._type;

        public static explicit operator int(SubAnimationType subAnimationType) => (int)subAnimationType._type;

        public static SubAnimationType GetSubAnimationType(int i)
        {
            if (!_types.TryGetValue(i, out var subAnimationType))
            {
                subAnimationType = new SubAnimationType((Type)i);
                _types.Add(i, subAnimationType);
            }

            return subAnimationType;
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