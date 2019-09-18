// ------------------------------------------------------------------------------
// <copyright file="MapFlags.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build
{
    [Flags]
    public enum MapFlags
    {
        HideMinimapInPreviewScreens = 0x0001,
        ModifyAllyPriorities = 0x0002,
        MeleeMap = 0x0004,
        PlayableMapSizeWasLarge = 0x0008,
        MaskedAreasArePartiallyVisible = 0x0010,
        FixedPlayerSettingsForCustomForces = 0x0020,
        UseCustomForces = 0x0040,
        UseCustomTechtree = 0x0080,
        UseCustomAbilities = 0x0100,
        UseCustomUpgrades = 0x0200,
        HasMapPropertiesMenuBeenOpened = 0x0400,
        ShowWaterWavesOnCliffShores = 0x0800,
        ShowWaterWavesOnRollingShores = 0x1000,

        // TFT only
        HasTerrainFog = 0x2000,
        RequiresExpansion = 0x4000,
        UseItemClassificationSystem = 0x8000,
    }
}