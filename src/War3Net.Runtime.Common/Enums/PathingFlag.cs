// ------------------------------------------------------------------------------
// <copyright file="PathingFlag.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class PathingFlag
    {
        private static readonly Dictionary<int, PathingFlag> _flags = GetTypes().ToDictionary(t => (int)t, t => new PathingFlag(t));

        private readonly Type _type;

        private PathingFlag(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Unwalkable = 1 << 1,
            Unflyable = 1 << 2,
            Unbuildable = 1 << 3,
            UnPeonHarvest = 1 << 4,
            Blighted = 1 << 5,
            Unfloatable = 1 << 6,
            Unamphibious = 1 << 7,
            UnItemPlacable = 1 << 8,
        }

        public static PathingFlag? GetPathingFlag(int i)
        {
            return _flags.TryGetValue(i, out var pathingFlag) ? pathingFlag : null;
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