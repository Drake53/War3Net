// ------------------------------------------------------------------------------
// <copyright file="Version.cs" company="Drake53">
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
    public sealed class Version : Handle
    {
        private static readonly Dictionary<int, Version> _versions = GetTypes().ToDictionary(t => (int)t, t => new Version(t));

        private readonly Type _type;

        private Version(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ReignOfChaos = 0,
            FrozenThrone = 1,
        }

        public static implicit operator Type(Version version) => version._type;

        public static explicit operator int(Version version) => (int)version._type;

        public static Version GetVersion(int i)
        {
            if (!_versions.TryGetValue(i, out var version))
            {
                version = new Version((Type)i);
                _versions.Add(i, version);
            }

            return version;
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