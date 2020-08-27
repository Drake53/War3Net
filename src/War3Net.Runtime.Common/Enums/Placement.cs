// ------------------------------------------------------------------------------
// <copyright file="Placement.cs" company="Drake53">
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
    public sealed class Placement
    {
        private static readonly Dictionary<int, Placement> _placements = GetTypes().ToDictionary(t => (int)t, t => new Placement(t));

        private readonly Type _type;

        private Placement(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Random = 0,
            Fixed = 1,
            UseMapSettings = 2,
            TeamsTogether = 3,
        }

        public static Placement GetPlacement(int i)
        {
            if (!_placements.TryGetValue(i, out var placement))
            {
                placement = new Placement((Type)i);
                _placements.Add(i, placement);
            }

            return placement;
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