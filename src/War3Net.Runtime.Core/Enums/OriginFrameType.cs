// ------------------------------------------------------------------------------
// <copyright file="OriginFrameType.cs" company="Drake53">
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
    public sealed class OriginFrameType : Handle
    {
        private static readonly Dictionary<int, OriginFrameType> _types = GetTypes().ToDictionary(t => (int)t, t => new OriginFrameType(t));

        private readonly Type _type;

        private OriginFrameType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            GameUI = 0,
            CommandButton = 1,
            HeroBar = 2,
            HeroButton = 3,
            HeroHPBar = 4,
            HeroManaBar = 5,
            HeroButtonIndicator = 6,
            ItemButton = 7,
            Minimap = 8,
            MinimapButton = 9,
            SystemButton = 10,
            Tooltip = 11,
            Ubertooltip = 12,
            ChatMessage = 13,
            UnitMessage = 14,
            TopMessage = 15,
            Portrait = 16,
            WorldFrame = 17,
        }

        public static implicit operator Type(OriginFrameType originFrameType) => originFrameType._type;

        public static explicit operator int(OriginFrameType originFrameType) => (int)originFrameType._type;

        public static OriginFrameType GetOriginFrameType(int i)
        {
            if (!_types.TryGetValue(i, out var originFrameType))
            {
                originFrameType = new OriginFrameType((Type)i);
                _types.Add(i, originFrameType);
            }

            return originFrameType;
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