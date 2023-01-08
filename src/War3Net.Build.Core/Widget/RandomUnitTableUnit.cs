// ------------------------------------------------------------------------------
// <copyright file="RandomUnitTableUnit.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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