// ------------------------------------------------------------------------------
// <copyright file="MapFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Info
{
    [Flags]
    public enum MapFlags
    {
        HideMinimapInPreviewScreens = 1 << 0,
        ModifyAllyPriorities = 1 << 1,
        MeleeMap = 1 << 2,
        PlayableMapSizeWasLarge = 1 << 3,
        MaskedAreasArePartiallyVisible = 1 << 4,
        FixedPlayerSettingsForCustomForces = 1 << 5,
        UseCustomForces = 1 << 6,
        UseCustomTechtree = 1 << 7,
        UseCustomAbilities = 1 << 8,
        UseCustomUpgrades = 1 << 9,
        HasMapPropertiesMenuBeenOpened = 1 << 10,
        ShowWaterWavesOnCliffShores = 1 << 11,
        ShowWaterWavesOnRollingShores = 1 << 12,

        // TFT only
        HasTerrainFog = 1 << 13,
        RequiresExpansion = 1 << 14,
        UseItemClassificationSystem = 1 << 15,
        HasWaterTintingColor = 1 << 16,

        // Reforged only
        AccurateProbabilityForCalculations = 1 << 17,
        CustomAbilitySkin = 1 << 18,

        Flag30 = 1 << 29,
        Flag31 = 1 << 30,
    }
}