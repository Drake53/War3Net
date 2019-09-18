// ------------------------------------------------------------------------------
// <copyright file="IMainFunctionBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    internal interface IMainFunctionBuilder<TStatementSyntax>
    {
        bool EnableCSharp { get; set; }

        TStatementSyntax GenerateSetCameraBoundsStatement(
               string functionName,
               string marginFunctionName,
               string marginLeft,
               string marginRight,
               string marginTop,
               string marginBottom,
               float x1,
               float y1,
               float x2,
               float y2,
               float x3,
               float y3,
               float x4,
               float y4);

        TStatementSyntax GenerateSetDayNightModelsStatement(
            string functionName,
            string terrainDNCFile,
            string unitDNCFile);

        TStatementSyntax GenerateSetTerrainFogExStatement(
            string functionName,
            int fogStyle,
            float startZ,
            float endZ,
            float density,
            float red,
            float green,
            float blue);

        TStatementSyntax GenerateAddWeatherEffectStatement(
            string functionName,
            string enableFunctionName,
            string rectFunctionName,
            float left,
            float bottom,
            float right,
            float top,
            int weatherType);

        TStatementSyntax GenerateSetMapMusicStatement(
            string functionName,
            string musicName,
            bool random,
            int index);
    }
}