using System.Drawing;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class SpecialDoodadData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialDoodadData"/> class.
        /// </summary>
        public SpecialDoodadData()
        {
        }

        public int TypeId { get; set; }

        public int Variation { get; set; }

        public Point Position { get; set; }

        public override string ToString() => TypeId.ToRawcode();
    }
}