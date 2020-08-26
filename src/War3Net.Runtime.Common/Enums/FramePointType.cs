﻿// ------------------------------------------------------------------------------
// <copyright file="FramePointType.cs" company="Drake53">
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
    public sealed class FramePointType
    {
        private static readonly Dictionary<int, FramePointType> _types = GetTypes().ToDictionary(t => (int)t, t => new FramePointType(t));

        private readonly Type _type;

        private FramePointType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            TopLeft = 0,
            Top = 1,
            TopRight = 2,

            Left = 3,
            Center = 4,
            Right = 5,

            BottomLeft = 6,
            Bottom = 7,
            BottomRight = 8,
        }

        public static FramePointType GetFramePointType(int i)
        {
            if (!_types.TryGetValue(i, out var framePointType))
            {
                framePointType = new FramePointType((Type)i);
                _types.Add(i, framePointType);
            }

            return framePointType;
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