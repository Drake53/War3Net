// ------------------------------------------------------------------------------
// <copyright file="CampaignFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build
{
    [Flags]
    public enum CampaignFiles
    {
        ImportedFiles = 1 << 0,
        Info = 1 << 1,
        AbilityObjectData = 1 << 2,
        BuffObjectData = 1 << 3,
        DestructableObjectData = 1 << 4,
        DoodadObjectData = 1 << 5,
        ItemObjectData = 1 << 6,
        UnitObjectData = 1 << 7,
        UpgradeObjectData = 1 << 8,
        TriggerStrings = 1 << 9,

        All = ImportedFiles
            | Info
            | AbilityObjectData
            | BuffObjectData
            | DestructableObjectData
            | DoodadObjectData
            | ItemObjectData
            | UnitObjectData
            | UpgradeObjectData
            | TriggerStrings,
    }
}