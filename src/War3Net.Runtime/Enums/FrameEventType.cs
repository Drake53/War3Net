// ------------------------------------------------------------------------------
// <copyright file="FrameEventType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums
{
    public sealed class FrameEventType
    {
        private static readonly Dictionary<int, FrameEventType> _types = GetTypes().ToDictionary(t => (int)t, t => new FrameEventType(t));

        private readonly Type _type;

        private FrameEventType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ControlClick = 1,
            MouseEnter = 2,
            MouseLeave = 3,
            MouseUp = 4,
            MouseDown = 5,
            MouseWheel = 6,
            CheckboxChecked = 7,
            CheckboxUnchecked = 8,
            EditboxTextChanged = 9,
            PopupMenuItemChanged = 10,
            MouseDoubleClick = 11,
            SpriteAnimationUpdate = 12,
            SliderValueChanged = 13,
            DialogCancel = 14,
            DialogAccept = 15,
            EditboxEnter = 16,
        }

        public static FrameEventType GetFrameEventType(int i)
        {
            if (!_types.TryGetValue(i, out var frameEventType))
            {
                frameEventType = new FrameEventType((Type)i);
                _types.Add(i, frameEventType);
            }

            return frameEventType;
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