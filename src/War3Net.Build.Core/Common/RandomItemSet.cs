// ------------------------------------------------------------------------------
// <copyright file="RandomItemSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Common
{
    public sealed partial class RandomItemSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomItemSet"/> class.
        /// </summary>
        public RandomItemSet()
        {
        }

        public List<RandomItemSetItem> Items { get; init; } = new();
    }
}