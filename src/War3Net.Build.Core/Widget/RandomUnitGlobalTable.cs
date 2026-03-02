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