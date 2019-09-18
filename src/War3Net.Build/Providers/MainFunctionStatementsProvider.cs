// ------------------------------------------------------------------------------
// <copyright file="MainFunctionStatementsProvider.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Script;

namespace War3Net.Build.Providers
{
    internal static class MainFunctionStatementsProvider<TBuilder, TStatementSyntax, TFunctionSyntax>
        where TBuilder : FunctionBuilder<TStatementSyntax, TFunctionSyntax>, IMainFunctionBuilder<TStatementSyntax>
    {
        private const string MainFunctionName = "main";

        private const string MusicName = "Music";
        private const bool MusicRandom = true;
        private const int MusicIndex = 0;

        public static string GetMainFunctionName => MainFunctionName;

        public static IEnumerable<TStatementSyntax> GetStatements(TBuilder builder)
        {
            var mapInfo = builder.MapInfo;

            yield return builder.GenerateSetCameraBoundsStatement(
                nameof(War3Api.Common.SetCameraBounds),
                nameof(War3Api.Common.GetCameraMargin),
                nameof(War3Api.Common.CAMERA_MARGIN_LEFT),
                nameof(War3Api.Common.CAMERA_MARGIN_RIGHT),
                nameof(War3Api.Common.CAMERA_MARGIN_TOP),
                nameof(War3Api.Common.CAMERA_MARGIN_BOTTOM),
                mapInfo.CameraBounds.BottomLeft.X,
                mapInfo.CameraBounds.BottomLeft.Y,
                mapInfo.CameraBounds.TopRight.X,
                mapInfo.CameraBounds.TopRight.Y,
                mapInfo.CameraBounds.TopLeft.X,
                mapInfo.CameraBounds.TopLeft.Y,
                mapInfo.CameraBounds.BottomRight.X,
                mapInfo.CameraBounds.BottomRight.Y);

            yield return builder.GenerateSetDayNightModelsStatement(
                nameof(War3Api.Common.SetDayNightModels),
                LightEnvironmentProvider.GetTerrainLightEnvironmentModel(mapInfo.LightEnvironment),
                LightEnvironmentProvider.GetUnitLightEnvironmentModel(mapInfo.LightEnvironment));

            if (mapInfo.MapFlags.HasFlag(MapFlags.HasTerrainFog))
            {
                yield return builder.GenerateSetTerrainFogExStatement(
                    nameof(War3Api.Common.SetTerrainFogEx),
                    (int)mapInfo.FogStyle,
                    mapInfo.FogStartZ,
                    mapInfo.FogEndZ,
                    mapInfo.FogDensity,
                    mapInfo.FogColor.R / 255f,
                    mapInfo.FogColor.G / 255f,
                    mapInfo.FogColor.B / 255f);
            }

            if (mapInfo.GlobalWeather != 0)
            {
                yield return builder.GenerateAddWeatherEffectStatement(
                    nameof(War3Api.Common.AddWeatherEffect),
                    nameof(War3Api.Common.EnableWeatherEffect),
                    nameof(War3Api.Common.Rect),
                    // TODO: use GetWorldBounds or get coords from w3i
                    -999999,
                    -999999,
                    999999,
                    999999,
                    mapInfo.GlobalWeather);
            }

            yield return builder.GenerateInvocationStatementWithStringArgument(
                nameof(War3Api.Common.NewSoundEnvironment),
                mapInfo.SoundEnvironment);

            yield return builder.GenerateInvocationStatementWithStringArgument(
                nameof(War3Api.Blizzard.SetAmbientDaySound),
                SoundEnvironmentProvider.GetAmbientDaySound(mapInfo.Tileset));

            yield return builder.GenerateInvocationStatementWithStringArgument(
                nameof(War3Api.Blizzard.SetAmbientNightSound),
                SoundEnvironmentProvider.GetAmbientNightSound(mapInfo.Tileset));

            yield return builder.GenerateSetMapMusicStatement(
                nameof(War3Api.Common.SetMapMusic),
                MusicName,
                MusicRandom,
                MusicIndex);

            yield return builder.GenerateInvocationStatementWithoutArguments(
                nameof(War3Api.Blizzard.InitBlizzard));

            if (builder.EnableCSharp)
            {
                // TODO: use constant on line 241 of LuaSyntaxGenerator: const string kManifestFuncName = "InitCSharp";
                yield return builder.GenerateInvocationStatementWithoutArguments(
                    "InitCSharp");
            }
        }
    }
}