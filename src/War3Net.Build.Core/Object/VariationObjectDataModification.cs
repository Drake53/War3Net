namespace War3Net.Build.Object
{
    public sealed partial class VariationObjectDataModification : ObjectDataModification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariationObjectDataModification"/> class.
        /// </summary>
        public VariationObjectDataModification()
        {
        }

        public int Variation { get; set; }

        public int Pointer { get; set; }
    }
}