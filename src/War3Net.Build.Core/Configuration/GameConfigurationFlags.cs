// ------------------------------------------------------------------------------
// <copyright file="GameConfigurationFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Configuration
{
    [Flags]
    public enum GameConfigurationFlags
    {
        IsFogOfWarDisabled = 1 << 0,
        IsVictoryDefeatConditionsDisabled = 1 << 1,
    }
}