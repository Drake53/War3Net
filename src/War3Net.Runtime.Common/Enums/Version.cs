// ------------------------------------------------------------------------------
// <copyright file="Version.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class Version
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