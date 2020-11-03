// ------------------------------------------------------------------------------
// <copyright file="StartLocationPriority.cs" company="Drake53">
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
    public sealed class StartLocationPriority : Handle
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

        public static implicit operator Type(StartLocationPriority startLocationPriority) => startLocationPriority._type;

        public static explicit operator int(StartLocationPriority startLocationPriority) => (int)startLocationPriority._type;

        public static StartLocationPriority GetStartLocationPriority(int i)
        {
            if (!_priorities.TryGetValue(i, out var startLocationPriority))
            {
                startLocationPriority = new StartLocationPriority((Type)i);
                _priorities.Add(i, startLocationPriority);
            }

            return startLocationPriority;
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