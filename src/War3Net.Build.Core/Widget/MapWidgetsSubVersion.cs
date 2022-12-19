// ------------------------------------------------------------------------------
// <copyright file="MapWidgetsSubVersion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1008
#pragma warning disable SA1300

namespace War3Net.Build.Widget
{
    /// <summary>
    /// Secondary file format version for <see cref="MapDoodads"/> and <see cref="MapUnits"/>.
    /// </summary>
    public enum MapWidgetsSubVersion
    {
        v7 = 7,
        v9 = 9,
        v10 = 10,
        v11 = 11,
    }
}