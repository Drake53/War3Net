// ------------------------------------------------------------------------------
// <copyright file="PathingType.cs" company="Drake53">
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
    public sealed class PathingType : Handle
    {
        private static readonly Dictionary<int, PathingType> _types = GetTypes().ToDictionary(t => (int)t, t => new PathingType(t));

        private readonly Type _type;

        private PathingType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Any = 0,
            Walkability = 1,
            Flyability = 2,
            Buildability = 3,
            PeonHarvestPathing = 4,
            BlightPathing = 5,
            Floatability = 6,
            AmphibiousPathing = 7,
        }

        public static implicit operator Type(PathingType pathingType) => pathingType._type;

        public static explicit operator int(PathingType pathingType) => (int)pathingType._type;

        public static PathingType GetPathingType(int i)
        {
            if (!_types.TryGetValue(i, out var pathingType))
            {
                pathingType = new PathingType((Type)i);
                _types.Add(i, pathingType);
            }

            return pathingType;
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