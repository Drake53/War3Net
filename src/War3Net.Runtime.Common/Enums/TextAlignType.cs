// ------------------------------------------------------------------------------
// <copyright file="TextAlignType.cs" company="Drake53">
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
    public sealed class TextAlignType
    {
        private static readonly Dictionary<int, TextAlignType> _types = GetTypes().ToDictionary(t => (int)t, t => new TextAlignType(t));

        private readonly Type _type;

        private TextAlignType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Top = 0,
            Middle = 1,
            Bottom = 2,

            Left = 3,
            Center = 4,
            Right = 5,
        }

        public static TextAlignType GetTextAlignType(int i)
        {
            if (!_types.TryGetValue(i, out var textAlignType))
            {
                textAlignType = new TextAlignType((Type)i);
                _types.Add(i, textAlignType);
            }

            return textAlignType;
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