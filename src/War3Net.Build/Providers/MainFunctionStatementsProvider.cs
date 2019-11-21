// ------------------------------------------------------------------------------
// <copyright file="MainFunctionStatementsProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Info;
using War3Net.Build.Script;

namespace War3Net.Build.Providers
{
    internal static class MainFunctionProvider
    {
        public const string FunctionName = "main";
        public const string LocalUnitVariableName = "u";
    }

    internal static class MainFunctionStatementsProvider<TBuilder, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TBuilder : FunctionBuilder<TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private const string MusicName = "Music";
        private const bool MusicRandom = true;
        private const int MusicIndex = 0;

        public static IEnumerable<TStatementSyntax> GetStatements(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetCameraBounds),
                builder.GenerateBinaryAdditionExpression(
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomLeft.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_LEFT)))),
                builder.GenerateBinaryAdditionExpression(
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomLeft.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_BOTTOM)))),
                builder.GenerateBinarySubtractionExpression(
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopRight.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_RIGHT)))),
                builder.GenerateBinarySubtractionExpression(
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopRight.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_TOP)))),
                builder.GenerateBinaryAdditionExpression(
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopLeft.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_LEFT)))),
                builder.GenerateBinarySubtractionExpression(
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopLeft.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_TOP)))),
                builder.GenerateBinarySubtractionExpression(
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomRight.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_RIGHT)))),
                builder.GenerateBinaryAdditionExpression(
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomRight.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_BOTTOM)))));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetDayNightModels),
                builder.GenerateStringLiteralExpression(LightEnvironmentProvider.GetTerrainLightEnvironmentModel(mapInfo.LightEnvironment)),
                builder.GenerateStringLiteralExpression(LightEnvironmentProvider.GetUnitLightEnvironmentModel(mapInfo.LightEnvironment)));

            if (mapInfo.MapFlags.HasFlag(MapFlags.HasTerrainFog))
            {
                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.SetTerrainFogEx),
                    builder.GenerateIntegerLiteralExpression((int)mapInfo.FogStyle),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogStartZ),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogEndZ),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogDensity),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogColor.R / 255f),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogColor.G / 255f),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogColor.B / 255f));
            }

            if (mapInfo.GlobalWeather != WeatherType.None)
            {
                // TODO: use GetWorldBounds or get coords from w3i/w3e
                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.EnableWeatherEffect),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.AddWeatherEffect),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.Rect),
                            builder.GenerateFloatLiteralExpression(-999999),
                            builder.GenerateFloatLiteralExpression(-999999),
                            builder.GenerateFloatLiteralExpression(999999),
                            builder.GenerateFloatLiteralExpression(999999)),
                        builder.GenerateIntegerLiteralExpression((int)mapInfo.GlobalWeather)),
                    builder.GenerateBooleanLiteralExpression(true));
            }

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.NewSoundEnvironment),
                builder.GenerateStringLiteralExpression(mapInfo.SoundEnvironment));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Blizzard.SetAmbientDaySound),
                builder.GenerateStringLiteralExpression(SoundEnvironmentProvider.GetAmbientDaySound(mapInfo.Tileset)));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Blizzard.SetAmbientNightSound),
                builder.GenerateStringLiteralExpression(SoundEnvironmentProvider.GetAmbientNightSound(mapInfo.Tileset)));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetMapMusic),
                builder.GenerateStringLiteralExpression(MusicName),
                builder.GenerateBooleanLiteralExpression(MusicRandom),
                builder.GenerateIntegerLiteralExpression(MusicIndex));

            if (builder.Data.MapUnits != null)
            {
                var localUnitDeclaration = builder.GenerateLocalDeclarationStatement(MainFunctionProvider.LocalUnitVariableName);
                if (localUnitDeclaration != null)
                {
                    yield return localUnitDeclaration;
                }

                foreach (var mapUnit in builder.Data.MapUnits)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.CreateUnit),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.Player),
                            builder.GenerateIntegerLiteralExpression(mapUnit.Owner)),
                        builder.GenerateFourCCExpression(mapUnit.TypeId),
                        builder.GenerateFloatLiteralExpression(mapUnit.PositionX),
                        builder.GenerateFloatLiteralExpression(mapUnit.PositionY),
                        builder.GenerateFloatLiteralExpression(mapUnit.Facing));

                    if (mapUnit.GoldAmount > 0)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetResourceAmount),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                            builder.GenerateIntegerLiteralExpression(mapUnit.GoldAmount));
                    }
                }
            }

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Blizzard.InitBlizzard));

            if (builder.Data.CSharp)
            {
                yield return builder.GenerateInvocationStatement(
                    CSharpLua.LuaSyntaxGenerator.kManifestFuncName);
            }
        }
    }
}