// ------------------------------------------------------------------------------
// <copyright file="MoveType.cs" company="Drake53">
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
    public sealed class MoveType : Handle
    {
        private static readonly Dictionary<int, MoveType> _types = GetTypes().ToDictionary(t => (int)t, t => new MoveType(t));

        private readonly Type _type;

        private MoveType(Type type)
        {
            _type = type;
        }

        [Flags]
        public enum Type
        {
            Unknown = 0,
            Foot = 1 << 0,
            Fly = 1 << 1,
            Horse = 1 << 2,
            Hover = 1 << 3,
            Float = 1 << 4,
            Amphibious = 1 << 5,
            Unbuildable = 1 << 6,
        }

        public static implicit operator Type(MoveType moveType) => moveType._type;

        public static explicit operator int(MoveType moveType) => (int)moveType._type;

        public static MoveType GetMoveType(int i)
        {
            if (!_types.TryGetValue(i, out var moveType))
            {
                moveType = new MoveType((Type)i);
                _types.Add(i, moveType);
            }

            return moveType;
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