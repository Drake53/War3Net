// ------------------------------------------------------------------------------
// <copyright file="RandomItemTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Common;

namespace War3Net.Build.Info
{
    public sealed partial class RandomItemTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomItemTable"/> class.
        /// </summary>
        public RandomItemTable()
        {
        }

        public int Index { get; set; }

        public string Name { get; set; }

        public List<RandomItemSet> ItemSets { get; init; } = new();

        public override string ToString() => Name;
    }
}