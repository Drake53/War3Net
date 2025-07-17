// ------------------------------------------------------------------------------
// <copyright file="MapInfoFormatVersion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1008
#pragma warning disable SA1300

using System.ComponentModel;

namespace War3Net.Build.Info
{
    /// <summary>
    /// File format version for <see cref="MapInfo"/>.
    /// </summary>
    public enum MapInfoFormatVersion
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        v8 = 8,

        [EditorBrowsable(EditorBrowsableState.Never)]
        v10 = 10,

        [EditorBrowsable(EditorBrowsableState.Never)]
        v11 = 11,

        [EditorBrowsable(EditorBrowsableState.Never)]
        v15 = 15,

        /// <summary>Reign of Chaos format.</summary>
        v18 = 18,

        [EditorBrowsable(EditorBrowsableState.Never)]
        v23 = 23,

        [EditorBrowsable(EditorBrowsableState.Never)]
        v24 = 24,

        /// <summary>The Frozen Throne format.</summary>
        v25 = 25,

        [EditorBrowsable(EditorBrowsableState.Never)]
        v26 = 26,

        [EditorBrowsable(EditorBrowsableState.Never)]
        v27 = 27,

        /// <summary>Introduced in patch 1.31.</summary>
        v28 = 28,

        /// <summary>Introduced in patch 1.32.</summary>
        v31 = 31,

        /// <summary>Introduced in patch 2.0.3 PTR.</summary>
        v32 = 32,

        /// <summary>Introduced in patch 2.0.3.</summary>
        v33 = 33,
    }
}