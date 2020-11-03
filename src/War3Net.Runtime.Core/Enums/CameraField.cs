// ------------------------------------------------------------------------------
// <copyright file="CameraField.cs" company="Drake53">
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
    public sealed class CameraField : Handle
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

        public static implicit operator Type(CameraField cameraField) => cameraField._type;

        public static explicit operator int(CameraField cameraField) => (int)cameraField._type;

        public static CameraField GetCameraField(int i)
        {
            if (!_fields.TryGetValue(i, out var cameraField))
            {
                cameraField = new CameraField((Type)i);
                _fields.Add(i, cameraField);
            }

            return cameraField;
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