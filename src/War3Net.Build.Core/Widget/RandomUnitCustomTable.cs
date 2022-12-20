// ------------------------------------------------------------------------------
// <copyright file="RandomUnitCustomTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Widget
{
    public sealed partial class RandomUnitCustomTable : RandomUnitData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomUnitCustomTable"/> class.
        /// </summary>
        public RandomUnitCustomTable()
        {
        }

        public List<RandomUnitTableUnit> RandomUnits { get; init; } = new();
    }
}