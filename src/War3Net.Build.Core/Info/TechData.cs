using War3Net.Build.Common;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class TechData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TechData"/> class.
        /// </summary>
        public TechData()
        {
        }

        public Bitmask32 Players { get; set; }

        public int Id { get; set; }

        public override string ToString() => Id.ToRawcode();
    }
}