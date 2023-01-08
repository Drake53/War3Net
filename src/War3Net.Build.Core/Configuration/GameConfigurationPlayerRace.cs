// ------------------------------------------------------------------------------
// <copyright file="GameConfigurationPlayerRace.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Configuration
{
    public enum GameConfigurationPlayerRace
    {
        Human = 1 << 0,
        Orc = 1 << 1,
        NightElf = 1 << 2,
        Undead = 1 << 3,
        Random = 1 << 5,
    }
}