// ------------------------------------------------------------------------------
// <copyright file="CreateCameras.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateCreateCameras(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapCameras = map.Cameras;
            if (mapCameras is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.CreateCameras}' cannot be generated without {nameof(MapCameras)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.CreateCameras);
            writer.WriteLine();

            foreach (var camera in mapCameras.Cameras)
            {
                var cameraName = camera.GetVariableName();

                writer.WriteSet(cameraName, JassExpression.InvokeSpaced(NativeName.CreateCameraSetup));
                writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.ZOffset, JassLiteral.Real(camera.ZOffset), "0.0");
                writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.Rotation, JassLiteral.Real(camera.Rotation), "0.0");
                writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.AngleOfAttack, JassLiteral.Real(camera.AngleOfAttack), "0.0");
                writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.TargetDistance, JassLiteral.Real(camera.TargetDistance), "0.0");
                writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.Roll, JassLiteral.Real(camera.Roll), "0.0");
                writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.FieldOfView, JassLiteral.Real(camera.FieldOfView), "0.0");
                writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.FarZ, JassLiteral.Real(camera.FarClippingPlane), "0.0");
                if (mapCameras.UseNewFormat)
                {
                    writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.NearZ, JassLiteral.Real(camera.NearClippingPlane), "0.0");
                    writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.LocalPitch, JassLiteral.Real(camera.LocalPitch), "0.0");
                    writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.LocalYaw, JassLiteral.Real(camera.LocalYaw), "0.0");
                    writer.WriteCall(NativeName.CameraSetupSetField, cameraName, CameraFieldName.LocalRoll, JassLiteral.Real(camera.LocalRoll), "0.0");
                }

                writer.WriteCall(NativeName.CameraSetupSetDestPosition, cameraName, JassLiteral.Real(camera.TargetPosition.X), JassLiteral.Real(camera.TargetPosition.Y), "0.0");
                writer.WriteLine();
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateCameras(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is not null && map.Info.FormatVersion == MapInfoFormatVersion.v8)
            {
                return true;
            }

            return map.Cameras is not null
                && map.Cameras.Cameras.Count > 0;
        }
    }
}