namespace War3Net.Build.Object
{
    public sealed partial class LevelObjectDataModification : ObjectDataModification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelObjectDataModification"/> class.
        /// </summary>
        public LevelObjectDataModification()
        {
        }

        public int Level { get; set; }

        public int Pointer { get; set; }
    }
}