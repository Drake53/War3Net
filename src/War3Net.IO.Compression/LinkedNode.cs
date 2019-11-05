// ------------------------------------------------------------------------------
// <copyright file="LinkedNode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#if NETCOREAPP3_0
using System.Diagnostics.CodeAnalysis;
#endif

namespace War3Net.IO.Compression
{
    /// <summary>
    /// A node which is both hierachcical (parent/child) and doubly linked (next/prev).
    /// </summary>
    internal class LinkedNode
    {
        private readonly int _decompressedValue;
        private int _weight;
        private LinkedNode? _next;
        private LinkedNode? _prev;
        private LinkedNode? _parent;
        private LinkedNode? _child0;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedNode"/> class.
        /// </summary>
        internal LinkedNode(int decompVal, int weight)
        {
            _decompressedValue = decompVal;
            _weight = weight;
        }

        internal int DecompressedValue => _decompressedValue;

        internal int Weight { get => _weight; set => _weight = value; }

#if NETCOREAPP3_0
        [DisallowNull]
#endif
        internal LinkedNode? Next { get => _next; set => _next = value; }

#if NETCOREAPP3_0
        [DisallowNull]
#endif
        internal LinkedNode? Prev { get => _prev; set => _prev = value; }

#if NETCOREAPP3_0
        [DisallowNull]
#endif
        internal LinkedNode? Parent { get => _parent; set => _parent = value; }

#if NETCOREAPP3_0
        [DisallowNull]
#endif
        internal LinkedNode? Child0 { get => _child0; set => _child0 = value; }

#if NETCOREAPP3_0
        [DisallowNull]
#endif
        internal LinkedNode? Child1 => _child0._prev;

        // TODO: This would be more efficient as a member of the other class
        // ie avoid the recursion
        internal LinkedNode Insert(LinkedNode other)
        {
            // 'Next' should have a lower weight
            // we should return the lower weight
            if (other._weight <= _weight)
            {
                // insert before
                if (_next != null)
                {
                    _next._prev = other;
                    other._next = _next;
                }

                _next = other;
                other._prev = this;
                return other;
            }
            else
            {
                if (_prev == null)
                {
                    // Insert after
                    other._prev = null;
                    _prev = other;
                    other._next = this;
                }
                else
                {
                    _prev.Insert(other);
                }
            }

            return this;
        }
    }
}