// ------------------------------------------------------------------------------
// <copyright file="MapWidgetsFormatVersion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1008
#pragma warning disable SA1300

using System.ComponentModel;

namespace War3Net.Build.Widget
{
    /// <summary>
    /// File format version for <see cref="MapDoodads"/> and <see cref="MapUnits"/>.
    /// </summary>
    public enum MapWidgetsFormatVersion
    {
        /// <summary>Reign of Chaos beta format.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        v6 = 6,

        /// <summary>Reign of Chaos format.</summary>
        v7 = 7,

        /// <summary>The Frozen Throne format.</summary>
        v8 = 8,
    }
}