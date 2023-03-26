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
using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax CreateCameras(Map map)
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

            var statements = new List<JassStatementSyntax>();
            //statements.Add(JassEmptySyntax.Value);

            var zero = SyntaxFactory.LiteralExpression(0f);

            foreach (var camera in mapCameras.Cameras)
            {
                var cameraName = camera.GetVariableName();

                statements.Add(SyntaxFactory.SetStatement(cameraName, SyntaxFactory.InvocationExpression(NativeName.CreateCameraSetup)));
                statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.ZOffset), SyntaxFactory.LiteralExpression(camera.ZOffset), zero));
                statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.Rotation), SyntaxFactory.LiteralExpression(camera.Rotation), zero));
                statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.AngleOfAttack), SyntaxFactory.LiteralExpression(camera.AngleOfAttack), zero));
                statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.TargetDistance), SyntaxFactory.LiteralExpression(camera.TargetDistance), zero));
                statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.Roll), SyntaxFactory.LiteralExpression(camera.Roll), zero));
                statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.FieldOfView), SyntaxFactory.LiteralExpression(camera.FieldOfView), zero));
                statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.FarZ), SyntaxFactory.LiteralExpression(camera.FarClippingPlane), zero));
                if (mapCameras.UseNewFormat)
                {
                    statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.NearZ), SyntaxFactory.LiteralExpression(camera.NearClippingPlane), zero));
                    statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.LocalPitch), SyntaxFactory.LiteralExpression(camera.LocalPitch), zero));
                    statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.LocalYaw), SyntaxFactory.LiteralExpression(camera.LocalYaw), zero));
                    statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetField, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.ParseIdentifierName(CameraFieldName.LocalRoll), SyntaxFactory.LiteralExpression(camera.LocalRoll), zero));
                }

                statements.Add(SyntaxFactory.CallStatement(NativeName.CameraSetupSetDestPosition, SyntaxFactory.ParseIdentifierName(cameraName), SyntaxFactory.LiteralExpression(camera.TargetPosition.X), SyntaxFactory.LiteralExpression(camera.TargetPosition.Y), zero));
                //statements.Add(JassEmptySyntax.Value);
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreateCameras)), statements);
        }

        protected internal virtual bool CreateCamerasCondition(Map map)
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
                && map.Cameras.Cameras.Any();
        }
    }
}