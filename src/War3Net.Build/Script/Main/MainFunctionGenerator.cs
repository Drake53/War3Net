// ------------------------------------------------------------------------------
// <copyright file="MainFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Widget;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TBuilder : FunctionBuilder<TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TGlobalDeclarationSyntax : class
        where TExpressionSyntax : class
    {
        private const string MusicName = "Music";
        private const bool MusicRandom = true;
        private const int MusicIndex = 0;

        private const string LocalItemIdVariableName = "itemID";
        private const string LocalTriggerVariableName = "t";

        private static bool WantGenerateSoundsHelperFunction(MapSounds mapSounds)
        {
            return (mapSounds?.Count ?? 0) > 0;
        }

        private static bool WantGenerateRegionsHelperFunction(MapRegions mapRegions)
        {
            return mapRegions?.Where(region => region.WeatherId != "\0\0\0\0" || region.AmbientSound != null).FirstOrDefault() != null;
        }

        private static bool WantGenerateCamerasHelperFunction(object unk)
        {
            return false;
        }

        private static bool WantGenerateUpgradesHelperFunction(MapInfo mapInfo)
        {
            return (mapInfo?.UpgradeDataCount ?? 0) > 0;
        }

        private static bool WantGenerateTechTreeHelperFunction(MapInfo mapInfo)
        {
            return (mapInfo?.TechDataCount ?? 0) > 0;
        }

        private static bool WantGenerateDestructablesHelperFunction(MapDoodads mapDoodads)
        {
            return (mapDoodads?.Count ?? 0) > 0;
        }

        private static bool WantGenerateItemsHelperFunction(MapUnits mapUnits)
        {
            return mapUnits?.Where(mapUnit => mapUnit.IsItem && mapUnit.TypeId != "sloc").FirstOrDefault() != null;
        }

        private static bool WantGenerateUnitsHelperFunction(MapUnits mapUnits)
        {
            return mapUnits?.Where(mapUnit => mapUnit.IsUnit).FirstOrDefault() != null;
        }

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

            var mapSounds = builder.Data.MapSounds;
            if (WantGenerateSoundsHelperFunction(mapSounds))
            {
                yield return GenerateCreateSoundsHelperFunction(builder);
            }

            var mapRegions = builder.Data.MapRegions;
            if (WantGenerateRegionsHelperFunction(mapRegions))
            {
                yield return GenerateCreateRegionsHelperFunction(builder);
            }

            if (WantGenerateCamerasHelperFunction(null))
            {
                // todo
            }

            if (WantGenerateUpgradesHelperFunction(mapInfo))
            {
                yield return GenerateUpgradesHelperFunction(builder);
            }

            if (WantGenerateTechTreeHelperFunction(mapInfo))
            {
                yield return GenerateTechTreeHelperFunction(builder);
            }

            var mapDoodads = builder.Data.MapDoodads;
            if (mapDoodads != null)
            {
                foreach (var doodad in mapDoodads.Where(mapDoodad => mapDoodad.DroppedItemData.FirstOrDefault() != default))
                {
                    yield return GenerateItemTableHelperFunction(builder, doodad);
                }

                if (WantGenerateDestructablesHelperFunction(mapDoodads))
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

                if (WantGenerateItemsHelperFunction(mapUnits))
                {
                    yield return GenerateCreateAllItemsHelperFunction(builder);
                }

                if (WantGenerateUnitsHelperFunction(mapUnits))
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
                builder.GenerateEscapedStringLiteralExpression(LightEnvironmentProvider.GetTerrainLightEnvironmentModel(mapInfo.LightEnvironment)),
                builder.GenerateEscapedStringLiteralExpression(LightEnvironmentProvider.GetUnitLightEnvironmentModel(mapInfo.LightEnvironment)));

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

            if (mapInfo.MapFlags.HasFlag(MapFlags.HasWaterTintingColor))
            {
                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.SetWaterBaseColor),
                    builder.GenerateIntegerLiteralExpression(mapInfo.WaterTintingColor.R),
                    builder.GenerateIntegerLiteralExpression(mapInfo.WaterTintingColor.G),
                    builder.GenerateIntegerLiteralExpression(mapInfo.WaterTintingColor.B),
                    builder.GenerateIntegerLiteralExpression(mapInfo.WaterTintingColor.A));
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
                builder.GenerateEscapedStringLiteralExpression(mapInfo.SoundEnvironment));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Blizzard.SetAmbientDaySound),
                builder.GenerateEscapedStringLiteralExpression(SoundEnvironmentProvider.GetAmbientDaySound(mapInfo.Tileset)));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Blizzard.SetAmbientNightSound),
                builder.GenerateEscapedStringLiteralExpression(SoundEnvironmentProvider.GetAmbientNightSound(mapInfo.Tileset)));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetMapMusic),
                builder.GenerateEscapedStringLiteralExpression(MusicName),
                builder.GenerateBooleanLiteralExpression(MusicRandom),
                builder.GenerateIntegerLiteralExpression(MusicIndex));

            var mapSounds = builder.Data.MapSounds;
            var mapRegions = builder.Data.MapRegions;
            var mapDoodads = builder.Data.MapDoodads;
            var mapUnits = builder.Data.MapUnits;

            if (WantGenerateSoundsHelperFunction(mapSounds))
            {
                yield return builder.GenerateInvocationStatement("InitSounds");
            }

            if (WantGenerateRegionsHelperFunction(mapRegions))
            {
                yield return builder.GenerateInvocationStatement("CreateRegions");
            }

            if (WantGenerateCamerasHelperFunction(null))
            {
                yield return builder.GenerateInvocationStatement("CreateCameras");
            }

            if (WantGenerateUpgradesHelperFunction(mapInfo))
            {
                yield return builder.GenerateInvocationStatement("InitUpgrades");
            }

            if (WantGenerateTechTreeHelperFunction(mapInfo))
            {
                yield return builder.GenerateInvocationStatement("InitTechTree");
            }

            if (WantGenerateDestructablesHelperFunction(mapDoodads))
            {
                yield return builder.GenerateInvocationStatement("CreateAllDestructables");
            }

            if (WantGenerateItemsHelperFunction(mapUnits))
            {
                yield return builder.GenerateInvocationStatement("CreateAllItems");
            }

            if (WantGenerateUnitsHelperFunction(mapUnits))
            {
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