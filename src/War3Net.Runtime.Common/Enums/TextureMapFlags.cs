// ------------------------------------------------------------------------------
// <copyright file="TextureMapFlags.cs" company="Drake53">
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
    public sealed class TextureMapFlags
    {
        private static readonly Dictionary<int, TextureMapFlags> _flags = GetTypes().ToDictionary(t => (int)t, t => new TextureMapFlags(t));

        private readonly Type _type;

        private TextureMapFlags(Type type)
        {
            _type = type;
        }

        [Flags]
        public enum Type
        {
            None = 0,
            WrapU = 1 << 0,
            WrapV = 1 << 1,
            WrapUV = WrapU | WrapV,
        }

        public static TextureMapFlags? GetTextureMapFlags(int i)
        {
            return _flags.TryGetValue(i, out var textureMapFlags) ? textureMapFlags : null;
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