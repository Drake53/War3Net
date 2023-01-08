// ------------------------------------------------------------------------------
// <copyright file="MapCustomTextTriggersFormatVersion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1300

namespace War3Net.Build.Script
{
    /// <summary>
    /// File format version for <see cref="MapCustomTextTriggers"/>.
    /// </summary>
    public enum MapCustomTextTriggersFormatVersion
    {
        /// <summary>Reign of Chaos format.</summary>
        v0 = 0,

        /// <summary>The Frozen Throne format.</summary>
        v1 = 1,
    }
}