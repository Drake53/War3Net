// ------------------------------------------------------------------------------
// <copyright file="TargetFlag.cs" company="Drake53">
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
    public sealed class TargetFlag
    {
        private static readonly Dictionary<int, TargetFlag> _flags = GetTypes().ToDictionary(t => (int)t, t => new TargetFlag(t));

        private readonly Type _type;

        private TargetFlag(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            None = 1 << 0,
            Ground = 1 << 1,
            Air = 1 << 2,
            Structure = 1 << 3,
            Ward = 1 << 4,
            Item = 1 << 5,
            Tree = 1 << 6,
            Wall = 1 << 7,
            Debris = 1 << 8,
            Decoration = 1 << 9,
            Bridge = 1 << 10,
        }

        public static TargetFlag? GetTargetFlag(int i)
        {
            return _flags.TryGetValue(i, out var targetFlag) ? targetFlag : null;
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