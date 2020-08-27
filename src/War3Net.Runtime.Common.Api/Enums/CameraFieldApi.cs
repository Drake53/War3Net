// ------------------------------------------------------------------------------
// <copyright file="CameraFieldApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
{
    public static class CameraFieldApi
    {
        public static readonly CameraField CAMERA_FIELD_TARGET_DISTANCE = ConvertCameraField((int)CameraField.Type.TargetDistance);
        public static readonly CameraField CAMERA_FIELD_FARZ = ConvertCameraField((int)CameraField.Type.FarZ);
        public static readonly CameraField CAMERA_FIELD_ANGLE_OF_ATTACK = ConvertCameraField((int)CameraField.Type.AngleOfAttack);
        public static readonly CameraField CAMERA_FIELD_FIELD_OF_VIEW = ConvertCameraField((int)CameraField.Type.FieldOfView);
        public static readonly CameraField CAMERA_FIELD_ROLL = ConvertCameraField((int)CameraField.Type.Roll);
        public static readonly CameraField CAMERA_FIELD_ROTATION = ConvertCameraField((int)CameraField.Type.Rotation);
        public static readonly CameraField CAMERA_FIELD_ZOFFSET = ConvertCameraField((int)CameraField.Type.ZOffset);
        public static readonly CameraField CAMERA_FIELD_NEARZ = ConvertCameraField((int)CameraField.Type.NearZ);
        public static readonly CameraField CAMERA_FIELD_LOCAL_PITCH = ConvertCameraField((int)CameraField.Type.LocalPitch);
        public static readonly CameraField CAMERA_FIELD_LOCAL_YAW = ConvertCameraField((int)CameraField.Type.LocalYaw);
        public static readonly CameraField CAMERA_FIELD_LOCAL_ROLL = ConvertCameraField((int)CameraField.Type.LocalRoll);

        public static CameraField ConvertCameraField(int i)
        {
            return CameraField.GetCameraField(i);
        }
    }
}