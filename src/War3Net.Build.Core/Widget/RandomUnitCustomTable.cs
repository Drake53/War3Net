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