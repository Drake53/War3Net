// ------------------------------------------------------------------------------
// <copyright file="CreateCameras.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Api.Common;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual JassFunctionDeclarationSyntax CreateCameras(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapCameras = map.Cameras;
            if (mapCameras is null)
            {
                throw new ArgumentException($"Function '{nameof(CreateCameras)}' cannot be generated without {nameof(MapCameras)}.", nameof(map));
            }

            var statements = new List<IStatementSyntax>();
            statements.Add(JassEmptyStatementSyntax.Value);

            var zero = SyntaxFactory.LiteralExpression(0f);

            foreach (var camera in mapCameras.Cameras)
            {
                var cameraName = camera.GetVariableName();

                statements.Add(SyntaxFactory.SetStatement(cameraName, SyntaxFactory.InvocationExpression(nameof(CreateCameraSetup))));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_ZOFFSET)), SyntaxFactory.LiteralExpression(camera.ZOffset), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_ROTATION)), SyntaxFactory.LiteralExpression(camera.Rotation), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_ANGLE_OF_ATTACK)), SyntaxFactory.LiteralExpression(camera.AngleOfAttack), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_TARGET_DISTANCE)), SyntaxFactory.LiteralExpression(camera.TargetDistance), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_ROLL)), SyntaxFactory.LiteralExpression(camera.Roll), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_FIELD_OF_VIEW)), SyntaxFactory.LiteralExpression(camera.FieldOfView), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_FARZ)), SyntaxFactory.LiteralExpression(camera.FarClippingPlane), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_NEARZ)), SyntaxFactory.LiteralExpression(camera.NearClippingPlane), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_LOCAL_PITCH)), SyntaxFactory.LiteralExpression(camera.LocalPitch), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_LOCAL_YAW)), SyntaxFactory.LiteralExpression(camera.LocalYaw), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetField), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.VariableReferenceExpression(nameof(CAMERA_FIELD_LOCAL_ROLL)), SyntaxFactory.LiteralExpression(camera.LocalRoll), zero));
                statements.Add(SyntaxFactory.CallStatement(nameof(CameraSetupSetDestPosition), SyntaxFactory.VariableReferenceExpression(cameraName), SyntaxFactory.LiteralExpression(camera.TargetPosition.X), SyntaxFactory.LiteralExpression(camera.TargetPosition.Y), zero));
                statements.Add(JassEmptyStatementSyntax.Value);
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreateCameras)), statements);
        }

        protected virtual bool CreateCamerasCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Cameras is not null
                && map.Cameras.Cameras.Any();
        }
    }
}