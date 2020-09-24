// ------------------------------------------------------------------------------
// <copyright file="WidgetEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Event
{
    public sealed class WidgetEvent : EventId
    {
        private static readonly Dictionary<int, WidgetEvent> _events = GetTypes().ToDictionary(t => (int)t, t => new WidgetEvent(t));

        private readonly Type _type;

        private WidgetEvent(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Death = 89,
        }

        public static WidgetEvent GetWidgetEvent(int i)
        {
            if (!_events.TryGetValue(i, out var widgetEvent))
            {
                widgetEvent = new WidgetEvent((Type)i);
                _events.Add(i, widgetEvent);
            }

            return widgetEvent;
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