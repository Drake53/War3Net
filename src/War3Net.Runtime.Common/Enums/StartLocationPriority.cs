// ------------------------------------------------------------------------------
// <copyright file="StartLocationPriority.cs" company="Drake53">
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
    public sealed class StartLocationPriority
    {
        private static readonly Dictionary<int, StartLocationPriority> _priorities = GetTypes().ToDictionary(t => (int)t, t => new StartLocationPriority(t));

        private readonly Type _type;

        private StartLocationPriority(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Low = 0,
            High = 1,
            Not = 2,
        }

        public static StartLocationPriority? GetStartLocationPriority(int i)
        {
            return _priorities.TryGetValue(i, out var startLocationPriority) ? startLocationPriority : null;
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