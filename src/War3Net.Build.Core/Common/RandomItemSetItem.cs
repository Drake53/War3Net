// ------------------------------------------------------------------------------
// <copyright file="RandomItemSetItem.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Common.Extensions;

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