// ------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Numerics;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed class Camera
    {
        private Vector2 _targetPosition;
        private float _zOffset;
        private float _rotation;
        private float _angleOfAttack;
        private float _targetDistance;
        private float _roll;
        private float _fieldOfView;
        private float _farClippingPlane;
        private float _nearClippingPlane;
        private float _localPitch;
        private float _localYaw;
        private float _localRoll;
        private string _name;

        public static Camera Parse(Stream stream, bool useNewFormat, bool leaveOpen)
        {
            var camera = new Camera();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                camera._targetPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                camera._zOffset = reader.ReadSingle();
                camera._rotation = reader.ReadSingle();
                camera._angleOfAttack = reader.ReadSingle();
                camera._targetDistance = reader.ReadSingle();
                camera._roll = reader.ReadSingle();
                camera._fieldOfView = reader.ReadSingle();
                camera._farClippingPlane = reader.ReadSingle();
                camera._nearClippingPlane = reader.ReadSingle();

                if (useNewFormat)
                {
                    camera._localPitch = reader.ReadSingle();
                    camera._localYaw = reader.ReadSingle();
                    camera._localRoll = reader.ReadSingle();
                }

                camera._name = reader.ReadChars();
                if (string.IsNullOrWhiteSpace(camera._name))
                {
                    throw new InvalidDataException($"Camera name must contain at least one non-whitespace character.");
                }
            }

            return camera;
        }

        public void SerializeTo(Stream stream, bool useNewFormat, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                WriteTo(writer, useNewFormat);
            }
        }

        public void WriteTo(BinaryWriter writer, bool useNewFormat)
        {
            writer.Write(_targetPosition.X);
            writer.Write(_targetPosition.Y);
            writer.Write(_zOffset);
            writer.Write(_rotation);
            writer.Write(_angleOfAttack);
            writer.Write(_targetDistance);
            writer.Write(_roll);
            writer.Write(_fieldOfView);
            writer.Write(_farClippingPlane);
            writer.Write(_nearClippingPlane);

            if (useNewFormat)
            {
                writer.Write(_localPitch);
                writer.Write(_localYaw);
                writer.Write(_localRoll);
            }

            writer.WriteString(_name);
        }
    }
}