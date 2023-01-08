// ------------------------------------------------------------------------------
// <copyright file="MapSoundsFormatVersion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1008
#pragma warning disable SA1300

namespace War3Net.Build.Audio
{
    /// <summary>
    /// File format version for <see cref="MapSounds"/>.
    /// </summary>
    public enum MapSoundsFormatVersion
    {
        /// <summary>The initial version.</summary>
        v1 = 1,

        /// <summary>Introduced in patch 1.32.</summary>
        v2 = 2,

        /// <summary>Introduced in patch 1.32.6.</summary>
        v3 = 3,
    }
}