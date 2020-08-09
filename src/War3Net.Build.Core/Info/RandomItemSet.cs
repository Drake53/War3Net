// ------------------------------------------------------------------------------
// <copyright file="RandomItemSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

namespace War3Net.Build.Info
{
    public sealed class RandomItemSet : IEnumerable<(int, char[])>
    {
        private readonly List<(int, char[])> _setItems;

        public RandomItemSet()
        {
            _setItems = new List<(int, char[])>();
        }

        public int Size => _setItems.Count;

        public void AddItem(int chance, char[] id)
        {
            _setItems.Add((chance, id));
        }

        public IEnumerator<(int, char[])> GetEnumerator()
        {
            return ((IEnumerable<(int, char[])>)_setItems).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<(int, char[])>)_setItems).GetEnumerator();
        }
    }
}