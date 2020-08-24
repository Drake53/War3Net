// ------------------------------------------------------------------------------
// <copyright file="DialogEvent.cs" company="Drake53">
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
    public sealed class DialogEvent : EventId
    {
        private static readonly Dictionary<int, DialogEvent> _events = GetTypes().ToDictionary(t => (int)t, t => new DialogEvent(t));

        private readonly Type _type;

        private DialogEvent(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ButtonClick = 90,
            Click = 91,
        }

        public static DialogEvent? GetDialogEvent(int i)
        {
            return _events.TryGetValue(i, out var dialogEvent) ? dialogEvent : null;
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