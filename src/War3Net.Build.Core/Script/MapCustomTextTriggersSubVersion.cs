// ------------------------------------------------------------------------------
// <copyright file="MapCustomTextTriggersSubVersion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1008
#pragma warning disable SA1300

using System.ComponentModel;

namespace War3Net.Build.Script
{
    /// <summary>
    /// Secondary file format version for <see cref="MapCustomTextTriggers"/>.
    /// </summary>
    public enum MapCustomTextTriggersSubVersion
    {
        /// <summary>The initial beta version.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        v1 = unchecked((int)0x80000001),

        /// <summary>The initial version.</summary>
        v4 = unchecked((int)0x80000004),
    }
}