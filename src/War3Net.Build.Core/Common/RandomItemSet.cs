namespace War3Net.Build.Common
{
    public sealed partial class RandomItemSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomItemSet"/> class.
        /// </summary>
        public RandomItemSet()
        {
        }

        public List<RandomItemSetItem> Items { get; init; } = new();
    }
}