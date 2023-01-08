// ------------------------------------------------------------------------------
// <copyright file="RandomUnitGlobalTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Widget
{
    public sealed partial class RandomUnitGlobalTable : RandomUnitData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomUnitGlobalTable"/> class.
        /// </summary>
        public RandomUnitGlobalTable()
        {
        }

        public int TableId { get; set; }

        public int Column { get; set; }
    }
}