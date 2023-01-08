// ------------------------------------------------------------------------------
// <copyright file="RandomUnitTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Info
{
    public sealed partial class RandomUnitTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomUnitTable"/> class.
        /// </summary>
        public RandomUnitTable()
        {
        }

        public int Index { get; set; }

        public string Name { get; set; }

        public List<WidgetType> Types { get; init; } = new();

        public List<RandomUnitSet> UnitSets { get; init; } = new();

        public override string ToString() => Name;
    }
}