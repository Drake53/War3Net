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