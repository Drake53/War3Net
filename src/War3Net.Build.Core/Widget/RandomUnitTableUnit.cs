using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class RandomUnitTableUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomUnitTableUnit"/> class.
        /// </summary>
        public RandomUnitTableUnit()
        {
        }

        public int UnitId { get; set; }

        public int Chance { get; set; }

        public override string ToString() => $"{UnitId.ToRawcode()} ({Chance}%)";
    }
}