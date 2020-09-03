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
        private const float DefaultZOffset = 0f;
        private const float DefaultTargetDistance = 1650f;
        private const float DefaultAngleOfAttack = 304f;
        private const float DefaultRotation = 90f;
        private const float DefaultRoll = 0f;
        private const float DefaultLocalPitch = 0f;
        private const float DefaultLocalYaw = 0f;
        private const float DefaultLocalRoll = 0f;
        private const float DefaultNearZ = 16f;
        private const float DefaultFarZ = 5000f;
        private const float DefaultFieldOfView = 70f;

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
        private bool _aspectRatioChanged;

        public Camera(float x, float y, uint windowWidth, uint windowHeight)
        {
            _positionX = new InternalCameraField(x);
            _positionY = new InternalCameraField(y);
            _zOffset = new InternalCameraField(DefaultZOffset);
            _targetDistance = new InternalCameraField(DefaultTargetDistance);
            _angleOfAttack = new InternalCameraField(DefaultAngleOfAttack);
            _rotation = new InternalCameraField(DefaultRotation);
            _roll = new InternalCameraField(DefaultRoll);
            _localPitch = new InternalCameraField(DefaultLocalPitch);
            _localYaw = new InternalCameraField(DefaultLocalYaw);
            _localRoll = new InternalCameraField(DefaultLocalRoll);
            _nearZ = new InternalCameraField(DefaultNearZ, true);
            _farZ = new InternalCameraField(DefaultFarZ, true);
            _fieldOfView = new InternalCameraField(DefaultFieldOfView, true);

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

        public float PositionZ => throw new NotImplementedException();

        public float EyeX => _eye.X;

        public float EyeY => _eye.Y;

        public float EyeZ => _eye.Z;

        public static void InitLocalCamera(float x, float y, uint windowWidth, uint windowHeight, out Matrix4x4 viewMatrix, out Matrix4x4 perspectiveMatrix)
        {
            if (_localCamera != null)
            {
                throw new InvalidOperationException("Local camera has already been initialized.");
            }

            _localCamera = new Camera(x, y, windowWidth, windowHeight);

            viewMatrix = _localCamera.GetViewMatrix();
            perspectiveMatrix = _localCamera.GetPerspectiveMatrix();
        }

        public void Pan(float x, float y, float duration)
        {
            SetField(_positionX, x, duration);
            SetField(_positionY, y, duration);
        }

        public float GetField(CameraField cameraField)
        {
            return _cameraFieldMappings.TryGetValue(cameraField, out var field) ? field.Value : default;
        }

        public void SetField(CameraField cameraField, float value, float duration)
        {
            if (_cameraFieldMappings.TryGetValue(cameraField, out var field))
            {
                SetField(field, value, duration);
            }
        }

        public void Update(float deltaSeconds)
        {
            var viewMatrixChanged = false;
            var perspectiveMatrixChanged = _aspectRatioChanged;

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

                return field.Update(deltaSeconds);
            });

            _updatingFields.IntersectWith(newUpdatingFields);

            if (viewMatrixChanged)
            {
                var viewMatrix = GetViewMatrix();
                ViewChanged?.Invoke(viewMatrix);
            }

            if (perspectiveMatrixChanged)
            {
                var perspectiveMatrix = GetPerspectiveMatrix();
                ProjectionChanged?.Invoke(perspectiveMatrix);
            }
        }

        public void UpdateAspectRatio(uint windowWidth, uint windowHeight)
        {
            AspectRatio = (float)windowWidth / windowHeight;
            _aspectRatioChanged = true;
        }

        private Matrix4x4 GetViewMatrix()
        {
            var rotation
                = Matrix4x4.CreateRotationY(DegToRad * _roll.GetNewValue())
                * Matrix4x4.CreateRotationX(DegToRad * _angleOfAttack.GetNewValue())
                * Matrix4x4.CreateRotationZ(DegToRad * (_rotation.GetNewValue() - 90));

            var quaternion = Quaternion.CreateFromRotationMatrix(rotation);
            var lookDirection = Vector3.Transform(Vector3.UnitY, quaternion);
            var upVector = Vector3.Transform(Vector3.UnitZ, quaternion);

            var locationZ = 0f;
            var target = new Vector3(_positionX.GetNewValue(), _positionY.GetNewValue(), locationZ + _zOffset.GetNewValue());

            _eye = target - (lookDirection * _targetDistance.GetNewValue());

            return Matrix4x4.CreateLookAt(_eye, target, upVector);
        }

        private Matrix4x4 GetPerspectiveMatrix()
        {
            _aspectRatioChanged = false;
            return Matrix4x4.CreatePerspectiveFieldOfView(_fieldOfView.GetNewValue() * DegToRad, AspectRatio, _nearZ.GetNewValue(), _farZ.GetNewValue());
        }

        private void SetField(InternalCameraField field, float value, float duration)
        {
            field.SetTarget(value, duration);
            _updatingFields.Add(field);
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