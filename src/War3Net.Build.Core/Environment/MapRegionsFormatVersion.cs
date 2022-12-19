// ------------------------------------------------------------------------------
// <copyright file="MapRegionsFormatVersion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1008
#pragma warning disable SA1300

using System.ComponentModel;

namespace War3Net.Build.Environment
{
    /// <summary>
    /// File format version for <see cref="MapRegions"/>.
    /// </summary>
    public enum MapRegionsFormatVersion
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        v2 = 2,

        [EditorBrowsable(EditorBrowsableState.Never)]
        v3 = 3,

        /// <summary>The initial version.</summary>
        v5 = 5,
    }
}