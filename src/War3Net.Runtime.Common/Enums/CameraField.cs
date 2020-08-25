// ------------------------------------------------------------------------------
// <copyright file="CameraField.cs" company="Drake53">
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
    public sealed class CameraField
    {
        private static readonly Dictionary<int, CameraField> _fields = GetTypes().ToDictionary(t => (int)t, t => new CameraField(t));

        private readonly Type _type;

        private CameraField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            TargetDistance = 0,
            FarZ = 1,
            AngleOfAttack = 2,
            FieldOfView = 3,
            Roll = 4,
            Rotation = 5,
            ZOffset = 6,
            NearZ = 7,
            LocalPitch = 8,
            LocalYaw = 9,
            LocalRoll = 10,
        }

        public static CameraField? GetCameraField(int i)
        {
            return _fields.TryGetValue(i, out var cameraField) ? cameraField : null;
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