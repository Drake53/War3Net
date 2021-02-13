// ------------------------------------------------------------------------------
// <copyright file="GamePatchExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Common;
using War3Net.Build.Providers;

namespace War3Net.Build.Extensions
{
    public static class GamePatchExtensions
    {
        public static Version ToVersion(this GamePatch gamePatch)
        {
            return GamePatchVersionProvider.GetGameVersion(gamePatch);
        }
    }
}