// ------------------------------------------------------------------------------
// <copyright file="RandomUnitAny.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Widget
{
    public sealed partial class RandomUnitAny : RandomUnitData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomUnitAny"/> class.
        /// </summary>
        public RandomUnitAny()
        {
        }

        /// <summary>
        /// Set to -1 for any level.
        /// </summary>
        public int Level { get; set; }

        public ItemClass Class { get; set; }

        public override string ToString() => $"Level {Level} ({Class})";
    }
}