// ------------------------------------------------------------------------------
// <copyright file="LimitOp.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Event
{
    public sealed class LimitOp : EventId
    {
        private static readonly Dictionary<int, LimitOp> _events = GetTypes().ToDictionary(t => (int)t, t => new LimitOp(t));

        private readonly Type _type;

        private LimitOp(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            LessThan = 0,
            LessThanOrEqual = 1,
            Equal = 2,
            GreaterThanOrEqual = 3,
            GreaterThan = 4,
            NotEqual = 5,
        }

        public static LimitOp GetLimitOp(int i)
        {
            if (!_events.TryGetValue(i, out var limitOp))
            {
                limitOp = new LimitOp((Type)i);
                _events.Add(i, limitOp);
            }

            return limitOp;
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