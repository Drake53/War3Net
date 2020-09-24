// ------------------------------------------------------------------------------
// <copyright file="VolumeGroup.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class VolumeGroup
    {
        private static readonly Dictionary<int, VolumeGroup> _groups = GetTypes().ToDictionary(t => (int)t, t => new VolumeGroup(t));

        private readonly Type _type;

        private VolumeGroup(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            UnitMovement = 0,
            UnitSounds = 1,
            Combat = 2,
            Spells = 3,
            UI = 4,
            Music = 5,
            AmbientSounds = 6,
            Fire = 7,
        }

        public static VolumeGroup GetVolumeGroup(int i)
        {
            if (!_groups.TryGetValue(i, out var volumeGroup))
            {
                volumeGroup = new VolumeGroup((Type)i);
                _groups.Add(i, volumeGroup);
            }

            return volumeGroup;
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