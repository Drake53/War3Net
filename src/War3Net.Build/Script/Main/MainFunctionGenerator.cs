// ------------------------------------------------------------------------------
// <copyright file="MainFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Info;
using War3Net.Build.Providers;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TBuilder : FunctionBuilder<TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TExpressionSyntax : class
    {
        private const string MusicName = "Music";
        private const bool MusicRandom = true;
        private const int MusicIndex = 0;

        private const string LocalItemIdVariableName = "itemID";
        private const string LocalTriggerVariableName = "t";

        public static IEnumerable<TFunctionSyntax> GetFunctions(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;
            if (mapInfo.RandomUnitTableCount > 0)
            {
                yield return GenerateUnitTableHelperFunction(builder);
            }

            for (var i = 0; i < mapInfo.RandomItemTableCount; i++)
            {
                yield return GenerateItemTableHelperFunction(builder, i);
            }

            var mapDoodads = builder.Data.MapDoodads;
            if (mapDoodads != null)
            {
                foreach (var doodad in mapDoodads.Where(mapDoodad => mapDoodad.DroppedItemData.FirstOrDefault() != default))
                {
                    yield return GenerateItemTableHelperFunction(builder, doodad);
                }

                if (mapDoodads.Count > 0)
                {
                    yield return GenerateCreateAllDestructablesHelperFunction(builder);
                }
            }

            var mapUnits = builder.Data.MapUnits;
            if (mapUnits != null)
            {
                foreach (var unit in mapUnits.Where(mapUnit => mapUnit.DroppedItemData.FirstOrDefault() != default))
                {
                    yield return GenerateItemTableHelperFunction(builder, unit);
                }

                if (mapUnits.Where(mapUnit => mapUnit.IsItem).FirstOrDefault() != default)
                {
                    yield return GenerateCreateAllItemsHelperFunction(builder);
                }

                if (mapUnits.Where(mapUnit => mapUnit.IsUnit).FirstOrDefault() != default)
                {
                    yield return GenerateCreateAllUnitsHelperFunction(builder);
                }
            }

            yield return builder.Build("main", GetStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetStatements(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetCameraBounds),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Addition,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomLeft.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_LEFT)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Addition,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomLeft.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_BOTTOM)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Subtraction,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopRight.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_RIGHT)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Subtraction,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopRight.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_TOP)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Addition,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopLeft.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_LEFT)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Subtraction,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopLeft.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_TOP)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Subtraction,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomRight.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_RIGHT)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Addition,
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

            if (builder.Data.MapDoodads != null)
            {
                yield return builder.GenerateInvocationStatement("CreateAllDestructables");
            }

            if (builder.Data.MapUnits != null)
            {
                yield return builder.GenerateInvocationStatement("CreateAllItems");
                yield return builder.GenerateInvocationStatement("CreateAllUnits");
            }

            yield return builder.GenerateInvocationStatement(nameof(War3Api.Blizzard.InitBlizzard));

            if (builder.Data.CSharp)
            {
                yield return builder.GenerateInvocationStatement(CSharpLua.LuaSyntaxGenerator.kManifestFuncName);
            }
        }
    }
}