using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class ModifiedAbilityData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedAbilityData"/> class.
        /// </summary>
        public ModifiedAbilityData()
        {
        }

        public int AbilityId { get; set; }

        public bool IsAutocastActive { get; set; }

        public int HeroAbilityLevel { get; set; }

        public override string ToString() => AbilityId.ToRawcode();
    }
}