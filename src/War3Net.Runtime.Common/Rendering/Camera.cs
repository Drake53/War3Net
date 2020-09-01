// ------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Rendering
{
    public sealed class Camera
    {
        private const float DegToRad = MathF.PI / 180f;

        private static Camera _localCamera;

        private readonly Dictionary<CameraField, InternalCameraField> _cameraFieldMappings;
        private readonly HashSet<InternalCameraField> _updatingFields;

        private readonly InternalCameraField _positionX;
        private readonly InternalCameraField _positionY;
        private readonly InternalCameraField _zOffset;
        private readonly InternalCameraField _targetDistance;
        private readonly InternalCameraField _angleOfAttack;
        private readonly InternalCameraField _rotation;
        private readonly InternalCameraField _roll;
        private readonly InternalCameraField _localPitch;
        private readonly InternalCameraField _localYaw;
        private readonly InternalCameraField _localRoll;
        private readonly InternalCameraField _nearZ;
        private readonly InternalCameraField _farZ;
        private readonly InternalCameraField _fieldOfView;

        private Vector3 _eye;

        public Camera(float x, float y, uint windowWidth, uint windowHeight)
        {
            _positionX = new InternalCameraField(x);
            _positionY = new InternalCameraField(y);
            _zOffset = new InternalCameraField(0f);
            _targetDistance = new InternalCameraField(1650f);
            _angleOfAttack = new InternalCameraField(304f);
            _rotation = new InternalCameraField(90f);
            _roll = new InternalCameraField(0f);
            _localPitch = new InternalCameraField(0f);
            _localYaw = new InternalCameraField(0f);
            _localRoll = new InternalCameraField(0f);
            _nearZ = new InternalCameraField(16f, true);
            _farZ = new InternalCameraField(5000f, true);
            _fieldOfView = new InternalCameraField(70f, true);

            _cameraFieldMappings = GetCameraFieldMappings();
            _updatingFields = new HashSet<InternalCameraField>();

            UpdateAspectRatio(windowWidth, windowHeight);
        }

        public event Action<Matrix4x4> ViewChanged;

        public event Action<Matrix4x4> ProjectionChanged;

        public static Camera LocalCamera => _localCamera;

        public float AspectRatio { get; private set; }

        public float PositionX => _positionX.Value;

        public float PositionY => _positionY.Value;

        public float ZOffset => _zOffset.Value;

        public float TargetDistance => _targetDistance.Value;

        public float AngleOfAttack => _angleOfAttack.Value;

        public float Rotation => _rotation.Value;

        public float Roll => _roll.Value;

        public float LocalPitch => _localPitch.Value;

        public float LocalYaw => _localYaw.Value;

        public float LocalRoll => _localRoll.Value;

        public float NearZ => _nearZ.Value;

        public float FarZ => _farZ.Value;

        public float FieldOfView => _fieldOfView.Value;

        public float EyeX => _eye.X;

        public float EyeY => _eye.Y;

        public float EyeZ => _eye.Z;

        public static void InitLocalCamera(float x, float y, uint windowWidth, uint windowHeight)
        {
            if (_localCamera != null)
            {
                throw new InvalidOperationException("Local camera has already been initialized.");
            }

            _localCamera = new Camera(x, y, windowWidth, windowHeight);
        }

        public void PanCamera(float x, float y, float duration)
        {
            _positionX.SetTarget(x, duration);
            _positionY.SetTarget(y, duration);

            _updatingFields.Add(_positionX);
            _updatingFields.Add(_positionY);
        }

        public void SetField(CameraField cameraField, float value, float duration)
        {
            if (_cameraFieldMappings.TryGetValue(cameraField, out var internalCameraField))
            {
                internalCameraField.SetTarget(value, duration);
                _updatingFields.Add(internalCameraField);
            }
        }

        public void Update(float deltaSeconds)
        {
            var viewMatrixChanged = false;
            var perspectiveMatrixChanged = false;

            var newUpdatingFields = _updatingFields.Where(field =>
            {
                if (field.AffectsPerspectiveMatrix)
                {
                    perspectiveMatrixChanged = true;
                }
                else
                {
                    viewMatrixChanged = true;
                }

                return !field.Update(deltaSeconds);
            });

            _updatingFields.IntersectWith(newUpdatingFields);

            if (viewMatrixChanged)
            {
                ViewChanged?.Invoke(GetViewMatrix());
            }

            if (perspectiveMatrixChanged)
            {
                ProjectionChanged?.Invoke(GetPerspectiveMatrix());
            }
        }

        public void UpdateAspectRatio(uint windowWidth, uint windowHeight)
        {
            AspectRatio = (float)windowWidth / windowHeight;
        }

        private Matrix4x4 GetViewMatrix()
        {
            var rotation = Matrix4x4.CreateRotationY(Roll * DegToRad) * Matrix4x4.CreateRotationX(AngleOfAttack * DegToRad) * Matrix4x4.CreateRotationZ((Rotation - 90) * DegToRad);
            var lookDirection = Vector3.Transform(Vector3.UnitY, Quaternion.CreateFromRotationMatrix(rotation));

            var locationZ = 0f;
            var target = new Vector3(PositionX, PositionY, locationZ + ZOffset);

            _eye = target - (Vector3.Normalize(lookDirection) * TargetDistance);

            return Matrix4x4.CreateLookAt(_eye, target, Vector3.UnitZ);
        }

        private Matrix4x4 GetPerspectiveMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView * DegToRad, AspectRatio, NearZ, FarZ);
        }

        private Dictionary<CameraField, InternalCameraField> GetCameraFieldMappings()
        {
            return new Dictionary<CameraField, InternalCameraField>
            {
                { CameraField.GetCameraField((int)CameraField.Type.TargetDistance), _targetDistance },
                { CameraField.GetCameraField((int)CameraField.Type.FarZ), _farZ },
                { CameraField.GetCameraField((int)CameraField.Type.AngleOfAttack), _angleOfAttack },
                { CameraField.GetCameraField((int)CameraField.Type.FieldOfView), _fieldOfView },
                { CameraField.GetCameraField((int)CameraField.Type.Roll), _roll },
                { CameraField.GetCameraField((int)CameraField.Type.Rotation), _rotation },
                { CameraField.GetCameraField((int)CameraField.Type.ZOffset), _zOffset },
                { CameraField.GetCameraField((int)CameraField.Type.NearZ), _nearZ },
                { CameraField.GetCameraField((int)CameraField.Type.LocalPitch), _localPitch },
                { CameraField.GetCameraField((int)CameraField.Type.LocalYaw), _localYaw },
                { CameraField.GetCameraField((int)CameraField.Type.LocalRoll), _localRoll },
            };
        }
    }
}