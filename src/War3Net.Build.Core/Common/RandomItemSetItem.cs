namespace War3Net.Build.Common
{
    public sealed partial class RandomItemSetItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomItemSetItem"/> class.
        /// </summary>
        public RandomItemSetItem()
        {
        }

        public int Chance { get; set; }

        public int ItemId { get; set; }

        public override string ToString() => $"{ItemId.ToRawcode()} ({Chance}%)";
    }
}