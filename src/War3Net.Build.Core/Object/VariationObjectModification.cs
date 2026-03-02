namespace War3Net.Build.Object
{
    public sealed partial class VariationObjectModification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariationObjectModification"/> class.
        /// </summary>
        public VariationObjectModification()
        {
        }

        public int OldId { get; set; }

        public int NewId { get; set; }

        public List<int> Unk { get; init; } = new();

        public List<VariationObjectDataModification> Modifications { get; init; } = new();

        public override string ToString() => NewId == 0 ? OldId.ToRawcode() : $"{NewId.ToRawcode()}:{OldId.ToRawcode()}";
    }
}