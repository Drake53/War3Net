﻿// ------------------------------------------------------------------------------
// <copyright file="CampaignFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Info
{
    [Flags]
    public enum CampaignFlags
    {
        VariableDifficultyLevels = 1 << 0,
        RequiresExpansion = 1 << 1,
    }
}