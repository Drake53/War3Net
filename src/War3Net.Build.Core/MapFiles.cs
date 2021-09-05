// ------------------------------------------------------------------------------
// <copyright file="MapFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build
{
    [Flags]
    public enum MapFiles
    {
        Sounds = 1 << 0,
        Cameras = 1 << 1,
        Environment = 1 << 2,
        PathingMap = 1 << 3,
        PreviewIcons = 1 << 4,
        Regions = 1 << 5,
        ShadowMap = 1 << 6,
        ImportedFiles = 1 << 7,
        Info = 1 << 8,
        AbilityObjectData = 1 << 9,
        BuffObjectData = 1 << 10,
        DestructableObjectData = 1 << 11,
        DoodadObjectData = 1 << 12,
        ItemObjectData = 1 << 13,
        UnitObjectData = 1 << 14,
        UpgradeObjectData = 1 << 15,
        CustomTextTriggers = 1 << 16,
        Script = 1 << 17,
        Triggers = 1 << 18,
        TriggerStrings = 1 << 19,
        Doodads = 1 << 20,
        Units = 1 << 21,

        All = Sounds
            | Cameras
            | Environment
            | PathingMap
            | PreviewIcons
            | Regions
            | ShadowMap
            | ImportedFiles
            | Info
            | AbilityObjectData
            | BuffObjectData
            | DestructableObjectData
            | DoodadObjectData
            | ItemObjectData
            | UnitObjectData
            | UpgradeObjectData
            | CustomTextTriggers
            | Script
            | Triggers
            | TriggerStrings
            | Doodads
            | Units,
    }
}