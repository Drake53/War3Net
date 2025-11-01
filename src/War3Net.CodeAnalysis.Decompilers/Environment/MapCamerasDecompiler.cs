// ------------------------------------------------------------------------------
// 
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// 
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.Build.Environment;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{

    public partial class JassScriptDecompiler
    {
        [RegisterStatementParser]
        internal void ParseCameraCreation(StatementParserInput input)
        {
            var variableAssignment = GetVariableAssignment(input.StatementChildren);
            if (variableAssignment == null || !variableAssignment.StartsWith("gg_cam_"))
            {
                return;
            }

            var camera = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "CreateCameraSetup")
            .SafeMapFirst(x =>
            {
                return new Camera
                {
                    NearClippingPlane = Context.Options.mapCamerasUseNewFormat ? default : 100f,
                };
            });

            if (camera != null)
            {
                camera.Name = variableAssignment["gg_cam_".Length..].Replace('_', ' ');
                Context.Add(camera, variableAssignment);
                Context.HandledStatements.Add(input.Statement);
            }
        }

        [RegisterStatementParser]
        internal void ParseCameraSetupSetField(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "CameraSetupSetField")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    FieldName = ((JassVariableReferenceExpressionSyntax)x.Arguments.Arguments[1]).IdentifierName.Name,
                    Value = x.Arguments.Arguments[2].GetValueOrDefault<float>(),
                    Duration = x.Arguments.Arguments[3].GetValueOrDefault<float>(),
                };
            });

            if (match != null)
            {
                var camera = Context.Get<Camera>(match.VariableName) ?? Context.GetLastCreated<Camera>();
                if (camera != null)
                {
                    var handled = true;
                    var value = match.Value;
                    switch (match.FieldName)
                    {
                        case "CAMERA_FIELD_ZOFFSET":
                            camera.ZOffset = value;
                            break;
                        case "CAMERA_FIELD_ROTATION":
                            camera.Rotation = value;
                            break;
                        case "CAMERA_FIELD_ANGLE_OF_ATTACK":
                            camera.AngleOfAttack = value;
                            break;
                        case "CAMERA_FIELD_TARGET_DISTANCE":
                            camera.TargetDistance = value;
                            break;
                        case "CAMERA_FIELD_ROLL":
                            camera.Roll = value;
                            break;
                        case "CAMERA_FIELD_FIELD_OF_VIEW":
                            camera.FieldOfView = value;
                            break;
                        case "CAMERA_FIELD_FARZ":
                            camera.FarClippingPlane = value;
                            break;
                        case "CAMERA_FIELD_NEARZ":
                            camera.NearClippingPlane = value;
                            break;
                        case "CAMERA_FIELD_LOCAL_PITCH":
                            camera.LocalPitch = value;
                            break;
                        case "CAMERA_FIELD_LOCAL_YAW":
                            camera.LocalYaw = value;
                            break;
                        case "CAMERA_FIELD_LOCAL_ROLL":
                            camera.LocalRoll = value;
                            break;
                        default:
                            handled = false;
                            break;
                    }

                    if (handled)
                    {
                        Context.HandledStatements.Add(input.Statement);
                    }
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseCameraSetupSetDestPosition(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "CameraSetupSetDestPosition")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    X = x.Arguments.Arguments[1].GetValueOrDefault<float>(),
                    Y = x.Arguments.Arguments[2].GetValueOrDefault<float>(),
                    Duration = x.Arguments.Arguments[3].GetValueOrDefault<float>()
                };
            });

            if (match != null)
            {
                var camera = Context.Get<Camera>(match.VariableName) ?? Context.GetLastCreated<Camera>();
                if (camera != null)
                {
                    camera.TargetPosition = new(match.X, match.Y);
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }
    }
}