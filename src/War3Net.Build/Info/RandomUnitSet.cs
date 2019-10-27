// ------------------------------------------------------------------------------
// <copyright file="RandomUnitSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

namespace War3Net.Build.Info
{
    public sealed class RandomUnitSet : IEnumerable<char[]>
    {
        private readonly int _chance;
        private readonly List<char[]> _unitIds;

        public RandomUnitSet(int chance)
        {
            _chance = chance;
            _unitIds = new List<char[]>();
        }

        public int Chance => _chance;

        public void AddId(char[] id)
        {
            _unitIds.Add(id);
        }

        public IEnumerator<char[]> GetEnumerator()
        {
            return ((IEnumerable<char[]>)_unitIds).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<char[]>)_unitIds).GetEnumerator();
        }
    }
}