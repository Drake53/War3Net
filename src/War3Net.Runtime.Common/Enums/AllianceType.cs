// ------------------------------------------------------------------------------
// <copyright file="AllianceType.cs" company="Drake53">
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
    public sealed class AllianceType
    {
        private static readonly Dictionary<int, AllianceType> _types = GetTypes().ToDictionary(t => (int)t, t => new AllianceType(t));

        private readonly Type _type;

        private AllianceType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Passive = 0,
            HelpRequest = 1,
            HelpResponse = 2,
            SharedXP = 3,
            SharedSpells = 4,
            SharedVision = 5,
            SharedControl = 6,
            SharedAdvancedControl = 7,
            Rescuable = 8,
            SharedVisionForced = 9,
        }

        public static AllianceType GetAllianceType(int i)
        {
            if (!_types.TryGetValue(i, out var allianceType))
            {
                allianceType = new AllianceType((Type)i);
                _types.Add(i, allianceType);
            }

            return allianceType;
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