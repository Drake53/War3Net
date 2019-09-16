// ------------------------------------------------------------------------------
// <copyright file="RandomItemSet.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

namespace War3Net.Build
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