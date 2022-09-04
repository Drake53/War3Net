// ------------------------------------------------------------------------------
// <copyright file="GameConfigurationPlayerInfoFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Configuration
{
    [Flags]
    public enum GameConfigurationPlayerInfoFlags
    {
        IsUser = 1 << 0,
        IsObserver = 1 << 1,
        LoadCustomAIFile = 1 << 2,
        AIFilePathIsAbsolute = 1 << 3,
    }
}