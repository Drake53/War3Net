// ------------------------------------------------------------------------------
// <copyright file="Main.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable IDE1006, SA1300

using System;
using System.Collections.Generic;

using War3Net.Build.Common;
using War3Net.Build.Providers;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax main(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapEnvironment = map.Environment;
            var mapInfo = map.Info;

            var statements = new List<IStatementSyntax>();

            if (UseWeatherEffectVariable && EnableGlobalWeatherEffectCondition(map))
            {
                statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(TypeName.WeatherEffect), VariableName.WeatherEffect));
            }

            if (mapInfo.CameraBoundsComplements is null)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetCameraBounds,
                    SyntaxFactory.LiteralExpression(mapInfo.CameraBounds.BottomLeft.X, precision: 1),
                    SyntaxFactory.LiteralExpression(mapInfo.CameraBounds.BottomLeft.Y, precision: 1),
                    SyntaxFactory.LiteralExpression(mapInfo.CameraBounds.TopRight.X, precision: 1),
                    SyntaxFactory.LiteralExpression(mapInfo.CameraBounds.TopRight.Y, precision: 1),
                    SyntaxFactory.LiteralExpression(mapInfo.CameraBounds.TopLeft.X, precision: 1),
                    SyntaxFactory.LiteralExpression(mapInfo.CameraBounds.TopLeft.Y, precision: 1),
                    SyntaxFactory.LiteralExpression(mapInfo.CameraBounds.BottomRight.X, precision: 1),
                    SyntaxFactory.LiteralExpression(mapInfo.CameraBounds.BottomRight.Y, precision: 1)));
            }
            else
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetCameraBounds,
                    SyntaxFactory.BinaryAdditionExpression(
                        SyntaxFactory.LiteralExpression(mapEnvironment.Left + (128 * mapInfo.CameraBoundsComplements.Left), precision: 1),
                        SyntaxFactory.InvocationExpression(NativeName.GetCameraMargin, SyntaxFactory.VariableReferenceExpression(CameraMarginName.Left))),
                    SyntaxFactory.BinaryAdditionExpression(
                        SyntaxFactory.LiteralExpression(mapEnvironment.Bottom + (128 * mapInfo.CameraBoundsComplements.Bottom), precision: 1),
                        SyntaxFactory.InvocationExpression(NativeName.GetCameraMargin, SyntaxFactory.VariableReferenceExpression(CameraMarginName.Bottom))),
                    SyntaxFactory.BinarySubtractionExpression(
                        SyntaxFactory.LiteralExpression(mapEnvironment.Right - (128 * mapInfo.CameraBoundsComplements.Right), precision: 1),
                        SyntaxFactory.InvocationExpression(NativeName.GetCameraMargin, SyntaxFactory.VariableReferenceExpression(CameraMarginName.Right))),
                    SyntaxFactory.BinarySubtractionExpression(
                        SyntaxFactory.LiteralExpression(mapEnvironment.Top - (128 * mapInfo.CameraBoundsComplements.Top), precision: 1),
                        SyntaxFactory.InvocationExpression(NativeName.GetCameraMargin, SyntaxFactory.VariableReferenceExpression(CameraMarginName.Top))),
                    SyntaxFactory.BinaryAdditionExpression(
                        SyntaxFactory.LiteralExpression(mapEnvironment.Left + (128 * mapInfo.CameraBoundsComplements.Left), precision: 1),
                        SyntaxFactory.InvocationExpression(NativeName.GetCameraMargin, SyntaxFactory.VariableReferenceExpression(CameraMarginName.Left))),
                    SyntaxFactory.BinarySubtractionExpression(
                        SyntaxFactory.LiteralExpression(mapEnvironment.Top - (128 * mapInfo.CameraBoundsComplements.Top), precision: 1),
                        SyntaxFactory.InvocationExpression(NativeName.GetCameraMargin, SyntaxFactory.VariableReferenceExpression(CameraMarginName.Top))),
                    SyntaxFactory.BinarySubtractionExpression(
                        SyntaxFactory.LiteralExpression(mapEnvironment.Right - (128 * mapInfo.CameraBoundsComplements.Right), precision: 1),
                        SyntaxFactory.InvocationExpression(NativeName.GetCameraMargin, SyntaxFactory.VariableReferenceExpression(CameraMarginName.Right))),
                    SyntaxFactory.BinaryAdditionExpression(
                        SyntaxFactory.LiteralExpression(mapEnvironment.Bottom + (128 * mapInfo.CameraBoundsComplements.Bottom), precision: 1),
                        SyntaxFactory.InvocationExpression(NativeName.GetCameraMargin, SyntaxFactory.VariableReferenceExpression(CameraMarginName.Bottom)))));
            }

            if (SetDayNightModelsCondition(map))
            {
                var lightEnvironment = mapInfo.LightEnvironment == Tileset.Unspecified ? mapInfo.Tileset : mapInfo.LightEnvironment;
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetDayNightModels,
                    SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(LightEnvironmentProvider.GetTerrainLightEnvironmentModel(lightEnvironment))),
                    SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(LightEnvironmentProvider.GetUnitLightEnvironmentModel(lightEnvironment)))));
            }

            if (SetTerrainFogExCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetTerrainFogEx,
                    SyntaxFactory.LiteralExpression((int)mapInfo.FogStyle),
                    SyntaxFactory.LiteralExpression(mapInfo.FogStartZ),
                    SyntaxFactory.LiteralExpression(mapInfo.FogEndZ),
                    SyntaxFactory.LiteralExpression(mapInfo.FogDensity),
                    SyntaxFactory.LiteralExpression(mapInfo.FogColor.R / 255f),
                    SyntaxFactory.LiteralExpression(mapInfo.FogColor.G / 255f),
                    SyntaxFactory.LiteralExpression(mapInfo.FogColor.B / 255f)));
            }

            if (SetWaterBaseColorCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetWaterBaseColor,
                    SyntaxFactory.LiteralExpression(mapInfo.WaterTintingColor.R),
                    SyntaxFactory.LiteralExpression(mapInfo.WaterTintingColor.G),
                    SyntaxFactory.LiteralExpression(mapInfo.WaterTintingColor.B),
                    SyntaxFactory.LiteralExpression(mapInfo.WaterTintingColor.A)));
            }

            if (EnableGlobalWeatherEffectCondition(map))
            {
                var createWeather = SyntaxFactory.InvocationExpression(
                    NativeName.AddWeatherEffect,
                    SyntaxFactory.InvocationExpression(
                        NativeName.Rect,
                        SyntaxFactory.LiteralExpression(mapEnvironment.Left, precision: 1),
                        SyntaxFactory.LiteralExpression(mapEnvironment.Bottom, precision: 1),
                        SyntaxFactory.LiteralExpression(mapEnvironment.Right, precision: 1),
                        SyntaxFactory.LiteralExpression(mapEnvironment.Top, precision: 1)),
                    SyntaxFactory.FourCCLiteralExpression((int)mapInfo.GlobalWeather));

                if (UseWeatherEffectVariable)
                {
                    statements.Add(SyntaxFactory.SetStatement(VariableName.WeatherEffect, createWeather));
                    statements.Add(SyntaxFactory.CallStatement(NativeName.EnableWeatherEffect, SyntaxFactory.VariableReferenceExpression(VariableName.WeatherEffect), SyntaxFactory.LiteralExpression(true)));
                }
                else
                {
                    statements.Add(SyntaxFactory.CallStatement(NativeName.EnableWeatherEffect, createWeather, SyntaxFactory.LiteralExpression(true)));
                }
            }

            if (NewSoundEnvironmentCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.NewSoundEnvironment,
                    SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(string.IsNullOrEmpty(mapInfo.SoundEnvironment) ? "Default" : mapInfo.SoundEnvironment))));
            }

            if (SetAmbientSoundCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(
                    FunctionName.SetAmbientDaySound,
                    SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(SoundEnvironmentProvider.GetAmbientDaySound(mapInfo.Tileset)))));

                statements.Add(SyntaxFactory.CallStatement(
                    FunctionName.SetAmbientNightSound,
                    SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(SoundEnvironmentProvider.GetAmbientNightSound(mapInfo.Tileset)))));
            }

            if (SetMapMusicCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetMapMusic,
                    SyntaxFactory.LiteralExpression("Music"),
                    SyntaxFactory.LiteralExpression(true),
                    SyntaxFactory.LiteralExpression(0)));
            }

            if (InitSoundsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(InitSounds)));
            }

            if (CreateRegionsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateRegions)));
            }

            if (CreateCamerasCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateCameras)));
            }

            if (InitUpgradesCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(InitUpgrades)));
            }

            if (InitTechTreeCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(InitTechTree)));
            }

            if (CreateAllDestructablesCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateAllDestructables)));
            }

            if (CreateAllItemsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateAllItems)));
            }

            if (InitRandomGroupsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(InitRandomGroups)));
            }

            if (CreateAllUnitsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateAllUnits)));
            }

            if (InitBlizzardCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(FunctionName.InitBlizzard));
            }

            if (UseCSharpLua)
            {
                statements.Add(SyntaxFactory.CallStatement(CSharpLua.LuaSyntaxGenerator.kManifestFuncName));
            }

            statements.Add(JassEmptyStatementSyntax.Value);

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(main)), statements);
        }

        protected internal virtual bool mainCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }
    }
}